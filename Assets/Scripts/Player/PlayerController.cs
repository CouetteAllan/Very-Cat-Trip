using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rayqdr.CatInputs;

public class PlayerController : MonoBehaviour
{
    private MInputsAction _inputs;
    public MInputsAction Inputs {
        get 
        {
            if( _inputs == null)
            {
                _inputs = new MInputsAction();
                _inputs.Enable();
            }

            return _inputs;
        }
    }

    private void Start()
    {
        if(_inputs == null)
        {
            _inputs = new MInputsAction();
            _inputs.Enable();
        }


            
    }
}
