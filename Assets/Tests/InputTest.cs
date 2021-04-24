using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScript : InputTestFixture
    {
        private const float MAX_DISTANCE_ERROR = 1.0f;

        Gamepad gamepad;
        Keyboard keyboard;
        Mouse mouse;

        bool isSceneLoaded;

        GameObject player;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            isSceneLoaded = true;
        }

        [SetUp]
        public void TestSetup()
        {
            Debug.Log("Setup input devices");

            gamepad = InputSystem.AddDevice<Gamepad>();
            keyboard = InputSystem.AddDevice<Keyboard>();
            mouse = InputSystem.AddDevice<Mouse>();

            Debug.Log("Load test scene");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private IEnumerator LoadScene(string sceneName)
        {
            isSceneLoaded = false;
            SceneManager.LoadScene(sceneName);

            // Wait just in case that the physics may need to settle after loading a scene
            yield return new WaitForSeconds(1);
        }

        private IEnumerator GetPlayer()
        {
            // Wait until scene is loaded to get the player
            yield return new WaitUntil(() => isSceneLoaded);

            player = GameObject.FindGameObjectWithTag("Player");

            Assert.NotNull(player, "Could not find player in scene");

            Debug.Log("Got GameObject as player: " + player.ToString());
        }

        /*
         * Movement input test for keyboard
         */
        [UnityTest]
        public IEnumerator Input_TestMovementConsistency()
        {
            yield return LoadScene("Test");
            yield return GetPlayer();

            // How long to hold each key
            float holdDuration = 0.5f;
            // How long to wait before checking the next key
            float waitDuration = 0.5f;

            // For checking difference between initial and final positions
            Vector3 initialPosition = player.transform.position;

            UnityEngine.InputSystem.Controls.KeyControl[] sequence =
            {
                // Forward/Backward
                keyboard.wKey,
                keyboard.sKey,

                // Backward/Forward
                keyboard.sKey,
                keyboard.wKey,

                // Left/Right
                keyboard.aKey,
                keyboard.dKey,

                // Right/Left
                keyboard.dKey,
                keyboard.aKey
            };

            Debug.Log("Testing movement consistency");

            foreach (var k in sequence)
            {
                Press(k);
                yield return new WaitForSeconds(holdDuration);
                Release(k);
                yield return new WaitForSeconds(waitDuration);
            }

            // Final wait for physics to settle
            yield return new WaitForSeconds(3);

            float distanceError = (player.transform.position - initialPosition).magnitude;

            if (distanceError < MAX_DISTANCE_ERROR)
            {
                Debug.Log("Distance error is below threshold of " + MAX_DISTANCE_ERROR);
            }
            else
            {
                Assert.Fail("Player did not return within " + MAX_DISTANCE_ERROR + " to initial position");
            }
        }

        /*
         * Movement input test for keyboard
         */
        [UnityTest]
        public IEnumerator SingleJumpTest()
        {
            yield return LoadScene("Test");
            yield return GetPlayer();

            // How long to hold each key
            float holdDuration = 0.5f;
            // How long to wait before checking the next key
            float waitDuration = 0.5f;

            // For checking difference between initial and final positions
            Vector3 initialPosition = player.transform.position;

            UnityEngine.InputSystem.Controls.KeyControl[] sequence =
            {
                // Test Normal Jump
                keyboard.spaceKey

            };

            Debug.Log("Testing movement consistency");

            foreach (var k in sequence)
            {
                Press(k);
                yield return new WaitForSeconds(holdDuration);
                Release(k);
            }
            Vector3 afterPos = player.transform.position;
            Assert.True(afterPos.y > initialPosition.y, "y was not greater after jumping");
            //Assert.True(afterPos.y > initialPosition.y);



            // Final wait for physics to settle
            //yield return new WaitForSeconds(3);

            //float distanceError = (player.transform.position - initialPosition).magnitude;

            //if (distanceError < MAX_DISTANCE_ERROR)
            //{
            //    Debug.Log("Distance error is below threshold of " + MAX_DISTANCE_ERROR);
            //}
            //else
            //{
            //    Assert.Fail("Player did not return within " + MAX_DISTANCE_ERROR + " to initial position");
            //}
        }

        /*
         * Test double jump works
         */
        [UnityTest]
        public IEnumerator DoubleJumpTest()
        {
            yield return LoadScene("Test");
            yield return GetPlayer();

            // How long to hold each key
            float holdDuration = 0.5f;
            // How long to wait before checking the next key
            float waitDuration = 0.5f;

            // For checking difference between initial and final positions
            Vector3 initialPosition = player.transform.position;

            UnityEngine.InputSystem.Controls.KeyControl[] sequence =
            {
                // Test Normal Jump
                keyboard.spaceKey,
                keyboard.spaceKey

            };

            Debug.Log("Testing movement consistency");
            Vector3 posAfterFirst = new Vector3(0, 100000, 0);
            foreach (var k in sequence)
            {
                Press(k);
                yield return new WaitForSeconds(holdDuration);
                Release(k);
                if (posAfterFirst.y > 1000)
                    posAfterFirst = player.transform.position;
            }
            Vector3 afterPos = player.transform.position;
            Assert.True(afterPos.y > posAfterFirst.y, "y was not greater after jumping");

        }

        /*
         * Test primary attack
         */
        [UnityTest]
        public IEnumerator PrimaryAttack()
        {
            yield return LoadScene("Test");
            yield return GetPlayer();

            // How long to hold each key
            float holdDuration = 2.5f;
            // How long to wait before checking the next key
            float waitDuration = 0.5f;

            // For checking difference between initial and final positions
            Vector3 initialPosition = player.transform.position;

            UnityEngine.InputSystem.Controls.ButtonControl[] sequence =
            {
                // Test Normal Jump
                mouse.leftButton

            };

            Debug.Log("Testing movement consistency");
            foreach (var k in sequence)
            {
                Press(k);
                yield return new WaitForSeconds(holdDuration);
                Release(k);
            }
            //var bulletCount = GameObject.FindObjectOfType()
            int bulletCount = 0;
            foreach (var a in GameObject.FindObjectsOfType(typeof(MonoBehaviour)))
            {
                if (a.name.Contains("Projectile"))
                {
                    bulletCount += 1;
                }
            }

            Debug.Log("bullet count " + bulletCount);


            Assert.Greater(bulletCount, 2);

        }

        [UnityTest]
        public IEnumerator SecondaryAttack()
        {
            yield return LoadScene("Test");
            yield return GetPlayer();

            // How long to hold each key
            float holdDuration = 2.5f;
            // How long to wait before checking the next key
            float waitDuration = 0.5f;

            // For checking difference between initial and final positions
            Vector3 initialPosition = player.transform.position;

            UnityEngine.InputSystem.Controls.ButtonControl[] sequence =
            {
                // Test Normal Jump
                mouse.rightButton,
                mouse.rightButton,
                mouse.rightButton,
                mouse.rightButton,
                mouse.rightButton,
                mouse.rightButton,

            };

            Debug.Log("Testing movement consistency");
            foreach (var k in sequence)
            {
                Press(k);

                Release(k);
                yield return new WaitForSeconds(.1f);
            }
            //var bulletCount = GameObject.FindObjectOfType()
            int bulletCount = 0;
            foreach (var a in GameObject.FindObjectsOfType(typeof(MonoBehaviour)))
            {
                if (a.name.Contains("Explosion"))
                {
                    bulletCount += 1;
                }
            }

            yield return new WaitForSeconds(1);


            Debug.Log("bullet count " + bulletCount);


            Assert.AreEqual(bulletCount, 3);

        }
    }
}
