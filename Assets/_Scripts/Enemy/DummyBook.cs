using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class DummyBook : Enemy
    {
        public DummyBook(float damageAmount, int Hitpoints) : base(damageAmount, Hitpoints) { }


        private Enemy _enemy;

        private void Start()
        {
            _enemy = new DummyBook(10f, 3);
        }

        private void Update()
        {

        }
    }
}
