using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.InputSystem;

public class MovementTest : InputTestFixture
{
    [Test]
    public void testJump()
    {
        //Assert.That(false, "first test");
        Mouse mouse = InputSystem.AddDevice<Mouse>();
        Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
        PressAndRelease(mouse.leftButton);

    }
}
