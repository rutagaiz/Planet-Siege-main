using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;

public class RutaScripts
{
    [Test]
    public void CanFindPlayerMovement()
    {
        var player = new GameObject().AddComponent<Player_movement>();
        Assert.IsNotNull(player);
    }
}
