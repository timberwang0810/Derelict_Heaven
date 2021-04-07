using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressurizer : Enemy
{
    public AudioSource walkAudio;

    public override Form GetForm()
    {
        return Form.pressurizer;
    }

    public override void ResetState()
    {
        // nothing
    }

    protected override void EnemyPhysicsUpdate()
    {
        // nothing
    }

    protected override void EnemyStart()
    {
        walkAudio.Play();
    }

    protected override void EnemyUpdate()
    {
        walkAudio.volume = SoundManager.S.sfxAudio.volume;
    }
}
