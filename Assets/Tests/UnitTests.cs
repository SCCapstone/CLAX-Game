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
    //[UnityTest]
    //public IEnumerator DummyTest()
    //{

    //    yield return null;
    //}

    /*
     * test that moving platform actually moves
     */
    [UnityTest]
    public IEnumerator MovingPlatformTest()
    {
        yield return LoadScene("Test");

        Debug.Log("testing moving platform");


        GameObject movingPlatformToTest = GameObject.Find("Platform(Moving)");
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

    /*
     * test that moving platform actually moves
     */
    [UnityTest]
    public IEnumerator FallingPlatformTest()
    {
        yield return LoadScene("Test");

        GameObject fallingPlatform = GameObject.Find("Platform(Falling)");
        //Debug.Log("found " + fallingPlatform);
        var player = GameObject.Find("Player");

        Debug.Log("testing falling platform");



        player.transform.position = fallingPlatform.transform.position + new Vector3(0, 2.5f, 0);
        yield return new WaitForSeconds(3);

        Vector3 firstPos = player.transform.position;
        Debug.Log("first pos " + firstPos);

        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(5);
            Vector3 secondPos = player.transform.position;

            Debug.Log("second pos " + secondPos);
            if (Mathf.Abs(secondPos.y - firstPos.y) > 1)
                Assert.Pass("platform fell!");
        }

        yield return null;
    }

    /*
     * test that destructable doors actually break
     */
    //[UnityTest]
    //public IEnumerator DoorBreaksTest()
    //{
    //    yield return LoadScene("Hub1");

    //    GameObject fallingPlatform = GameObject.Find("Platform(Falling)");
    //    //Debug.Log("found " + fallingPlatform);
    //    var player = GameObject.Find("Player");
    //    player.transform.position = fallingPlatform.transform.position + new Vector3(0, 3, 0);

    //    Vector3 firstPos = fallingPlatform.transform.position;
    //    Debug.Log("first pos " + firstPos);

    //    yield return new WaitForSeconds(8);
    //    Vector3 secondPos = fallingPlatform.transform.position;

    //    Debug.Log("second pos " + secondPos);
    //    Assert.Less(secondPos.y, firstPos.y, "platform did not move!");

    //    yield return null;
    //}

    /*
     * test that the pill boss moves towards the player
     */
    [UnityTest]
    public IEnumerator PillBossMovesTowardPlayer()
    {
        yield return LoadScene("PillBossScene");
        //yield return new WaitForSeconds(1);
        //Instantiate(Resources.Load())
        //Debug.Log("found " + fallingPlatform);
        var boss = GameObject.Find("Boss2");
        var player = GameObject.FindGameObjectWithTag("Player");
        //player.transform.position = fallingPlatform.transform.position + new Vector3(0, 3, 0);
        Debug.Log("testing pill boss moves toward player");

        Vector3 firstPos = boss.transform.position;

        var distToPlayerBefore = Vector3.Distance(player.transform.position, firstPos);

        Debug.Log("first pos " + firstPos);

        yield return new WaitForSeconds(5);
        Vector3 secondPos = boss.transform.position;
        var distToPlayerAfter = Vector3.Distance(player.transform.position, secondPos);


        Debug.Log("second pos " + secondPos);
        Assert.Less(distToPlayerAfter * 2, distToPlayerBefore, "boss did not move");

        yield return null;
    }

    /*
     * test that the cube boss attacks the player
     */
    [UnityTest]
    public IEnumerator CubeBossAttacksPlayer()
    {
        yield return LoadScene("CubeBossScene");
        //yield return new WaitForSeconds(1);
        //Instantiate(Resources.Load())
        //Debug.Log("found " + fallingPlatform);
        var boss = GameObject.Find("CubeBoss");
        var player = GameObject.FindGameObjectWithTag("Player");

        Debug.Log("testing cube boss attacks player");

        //player.transform.position = fallingPlatform.transform.position + new Vector3(0, 3, 0);
        float waitTime = 2;
        for (int i = 0; i < 3; i++)
        {

            yield return new WaitForSeconds(1);

            var attacks = GameObject.FindGameObjectsWithTag("EnemyAttack");
            int numNearPlayer = 0;
            foreach (var attack in attacks)
            {
                if (Vector3.Distance(attack.transform.position, player.transform.position) < 15)
                {
                    numNearPlayer += 1;
                }
            }
            if (numNearPlayer >= 3)
                Assert.Pass("attack was launched at some point");
            else
                waitTime += 1;
        }


    }
    /*
     * test that the triangle boss moves
     */
    [UnityTest]
    public IEnumerator TriangleBossAttacksPlayer()
    {
        yield return LoadScene("TriangleBossScene");
        yield return new WaitForSeconds(1);
        //Instantiate(Resources.Load())
        //Debug.Log("found " + fallingPlatform);
        var boss = GameObject.Find("TriangleBoss");
        var player = GameObject.FindGameObjectWithTag("Player");

        Debug.Log("testing triangle boss moves");

        //player.transform.position = fallingPlatform.transform.position + new Vector3(0, 3, 0);

        Vector3 firstPos = boss.transform.position;

        Debug.Log("first pos " + firstPos);

        yield return new WaitForSeconds(2);
        Vector3 secondPos = boss.transform.position;

        Debug.Log("second pos " + secondPos);
        Assert.That(firstPos != secondPos, "boss did not move");

        yield return null;
    }

}
