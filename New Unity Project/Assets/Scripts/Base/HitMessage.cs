using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType { normal, acceleration };
public enum InfluenceType { none, instant, overtime};
public class HitMessage
{
    public Vector3 direction;
    public VelocityInfluence influence;
    public bool isPlayer;
    public bool muteVelocity;
    public FighterFilter sender;
    public ShotType shot;

    public HitMessage(Vector3 hitDirection, VelocityInfluence velocityInfluence, bool slowDownVelocity, FighterFilter sentBy, ShotType shotType)
    {
        direction = hitDirection;
        influence = velocityInfluence;
        isPlayer = sentBy != FighterFilter.both;
        muteVelocity = slowDownVelocity;
        sender = sentBy;
        shot = shotType;
    }
}

public class VelocityInfluence
{
    public Vector3 velocity;
    public InfluenceType type;

    public VelocityInfluence(Vector3 extraInfluence, InfluenceType influenceType) {
        velocity = extraInfluence;
        type = influenceType;
    }

    public VelocityInfluence() {
        velocity = Vector3.zero;
        type = InfluenceType.none;
    }
}
