using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Editor.PlayModeTests
{
    public class CoinTest
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator HeroCollectsCoinTest()
        {
            SceneManager.LoadScene("TestScene - Level1");
            yield return null;

            var coinTestBehaviour = new MonoBehaviourTest<CoinTestBehaviour>();
            yield return coinTestBehaviour;

            var coin = GameObject.FindWithTag("Coin");
            Assert.IsTrue(coin == null, "Coin is still on the scene");
        }
        
        [UnityTest]
        public IEnumerator CoinIncreaseTest()
        {
            SceneManager.LoadScene("TestScene - Level1");
            yield return null;

            var coinTestBehaviour = new MonoBehaviourTest<CoinTestBehaviour>();
            yield return coinTestBehaviour;
            
            LogAssert.Expect(LogType.Log, "Coins: 1");
        }
    }
}
