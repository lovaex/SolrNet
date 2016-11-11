using NUnit.Framework;
using SolrNet.Exceptions;

namespace SolrNet.Tests {
    [TestFixture]
    public class SolrMoreLikeThisHandlerStreamUrlQueryTests {
        [Test]
        public void Invalid_url_as_string_throws() {
            Assert.Throws<InvalidURLException>(() => {
                new SolrMoreLikeThisHandlerStreamUrlQuery("asdasd");
            });
        }
    }
}