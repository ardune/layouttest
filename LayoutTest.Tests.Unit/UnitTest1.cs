using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using LayoutTest.Features.Shared.State;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LayoutTest.Tests.Unit
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var appState = new AppStateHolder();

            void PagesTest(IEnumerable<PageTagView> results)
            {
                var sw = Stopwatch.StartNew();
                var data = results.Select(x=>new
                {
                    x.Page,
                    Tags = x.Tags.ToArray()
                }).ToArray();
                Trace.WriteLine("got " + data.Length + " in " + sw.ElapsedMilliseconds);
                
            }

            using (appState.CurrentState
                .DistinctUntilChanged(x=>new{x.Tags,x.PageTags,x.Pages})
                .Select(Project)
                .DistinctUntilChanged()
                .Subscribe(PagesTest))
            {
                int chunkSize = 50;
                int tagDensity = 10;

                for (int i = 0; i < 200; i++)
                {
                    var startId = i * chunkSize;
                    var finishId = (i + 1) * chunkSize;
                    var newPages = Enumerable.Range(startId, chunkSize).Select(x => new Page { Id = x }).ToArray();
                    var newTags = Enumerable.Range(startId, chunkSize).Select(x => new Tag { Id = x }).ToArray();
                    var newPageTags = newPages.SelectMany(page => newTags.Where(tag => (tag.Id + page.Id) % tagDensity == 0).Select(tag => new PageTag { PageId = page.Id, TagId = tag.Id })).ToArray();
                    var sw = Stopwatch.StartNew();

                    Trace.WriteLine("starting update " + i);
                    await appState.UpdateState(builder =>
                    {
                        builder.Pages = builder.Pages.Concat(newPages).ToArray();
                        builder.Tags = builder.Tags.Concat(newTags).ToArray();
                        builder.PageTags = builder.PageTags.Concat(newPageTags).ToArray();

                        return builder;
                    });

                    Trace.WriteLine("completed in: " + sw.ElapsedMilliseconds);
                }
            }
        }

        private static IEnumerable<PageTagView> Project(AppState state)
        {
            return state.Pages.Select(page => new PageTagView
            {
                Page = page,
                Tags = state.PageTags.Where(x => x.PageId == page.Id).SelectMany(pageTag => state.Tags.Where(tag => tag.Id == pageTag.TagId))
            });
        }
    }
    
}
