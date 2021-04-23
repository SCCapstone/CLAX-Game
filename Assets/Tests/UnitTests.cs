using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine.SceneManagement;

public class UnitTests : MonoBehaviour
{
    bool isSceneLoaded;
    float initialDelay = 2;

    //public GameObject movingPlatformToTest;

    [SetUp]
    public void TestSetup()
    {
        Debug.Log("Setup input devices");

        Debug.Log("Load test scene");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isSceneLoaded = true;
    }

    private IEnumerator LoadScene(string sceneName)
    {
        isSceneLoaded = false;
        SceneManager.LoadScene(sceneName);

        // Wait just in case that the physics may need to settle after loading a scene
        yield return new WaitForSeconds(initialDelay);
    }

    //private IEnumerator GetBoss()
    //{
    //    // Wait until scene is loaded to get the player
    //    yield return new WaitUntil(() => isSceneLoaded);

    //    //cubeBoss = GameObject.FindGameObjectWithTag("Player");

    //    //Assert.Pass();
    //    Assert.Pass();


    //    //Assert.NotNull(cubeBoss, "Could not find cubeBoss in scene");

    //    //Debug.Log("Got GameObject as cubeBoss: " + cubeBoss.ToString());
    //}
    [UnityTest]
    public IEnumerator DummyTest()
    {

        yield return null;
    }

    [UnityTest]//test that moving platform actually moves
    public IEnumerator PlatformTest()
    {
        yield return LoadScene("Test");

        GameObject movingPlatformToTest = GameObject.Find("Platform");
        Debug.Log("found " + movingPlatformToTest);

        var waitTime = 2;
        Vector3 firstPos = movingPlatformToTest.transform.position;
        Debug.Log("first pos " + firstPos);

        yield return new WaitForSeconds(waitTime);
        Vector3 secondPos = movingPlatformToTest.transform.position;

        Debug.Log("second pos " + secondPos);
        Assert.AreNotEqual(firstPos, movingPlatformToTest.transform.position, "platform did not move!");


        yield return null;
    }


}
