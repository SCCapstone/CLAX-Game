using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine.SceneManagement;

public class CubeBossTest
{
    bool isSceneLoaded;

    GameObject cubeBoss;

    float initialDelay = 3;

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

    private IEnumerator GetBoss()
    {
        // Wait until scene is loaded to get the player
        yield return new WaitUntil(() => isSceneLoaded);

        cubeBoss = GameObject.FindGameObjectWithTag("Player");

        Assert.NotNull(cubeBoss, "Could not find cubeBoss in scene");

        Debug.Log("Got GameObject as cubeBoss: " + cubeBoss.ToString());
    }

    [UnityTest]
    public IEnumerator CubeBossAttackTest()
    {
        yield return LoadScene("Test");
        yield return GetBoss();
        //Assert.AreEqual(5, 6);
        float delayBeforeAllAtGoal = 6.5f + initialDelay;

        yield return new WaitForSeconds(delayBeforeAllAtGoal);
        var cubeAttacks = GameObject.FindGameObjectsWithTag("cubeAttack");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //check that all of the cubes fired reached the player before the next batch
        var spaceAllowedBetween = .5;
        foreach (var cube in cubeAttacks)
        {
            var realSpaceBetween = Vector3.Distance(cube.transform.position, player.transform.position);
            if (realSpaceBetween > spaceAllowedBetween)
            {
                Assert.Fail("cube was too far from goal. Space was " + realSpaceBetween);
            }
        }
        Debug.Log("all cubes were within range");



        yield return null;
    }


}
