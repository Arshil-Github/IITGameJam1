using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner: MonoBehaviour
{
    public void instantiateEffect(GameObject _effect, float _destroyAfter, Vector2 _spawnPosition, int _scale)
    {
        GameObject e = Instantiate(_effect);
        e.transform.position = _spawnPosition;
        e.transform.localScale = new Vector3(_scale,  _scale);


        Destroy(e, _destroyAfter);

    }
}
