using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using GHApp.Contracts;
using GHApp.Contracts.Dto;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

namespace GHApp.Service
{
    [TestFixture]
    public class RepoWatcherTest
    {
        private Mock<IGithubBrowser> _browserMock;

        [SetUp]
        public void Setup()
        {
            _browserMock = new Mock<IGithubBrowser>();
            _browserMock
                .Setup(x => x.GetCommits(It.IsAny<Repo>()))
                .Returns(Observable.Empty<IEnumerable<Commit>>());
        }

        [Test]
        public void Should_Get_Commits_Every_10_seconds()
        {
            var someRepo = new Repo();
            TestScheduler testScheduler = new TestScheduler();

            var sut = new RepoWatcher(_browserMock.Object, someRepo, testScheduler);
            _browserMock.Verify(x => x.GetCommits(someRepo), Times.Never);
            testScheduler.AdvanceBy(TimeSpan.FromSeconds(10).Ticks);

            _browserMock.Verify(x => x.GetCommits(someRepo), Times.Once);
        }

    }
}