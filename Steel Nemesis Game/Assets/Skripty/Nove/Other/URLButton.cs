using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Other
{
    public class URLButton : MonoBehaviour
    {
        public void GoToWebsite(string site)
        {
            Application.OpenURL(site);
        }
    }
}
