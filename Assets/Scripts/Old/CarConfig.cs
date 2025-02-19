using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CarConfig
{
    public string version { get; set; }
    public string desc { get; set; }
    public List<string> drive { get; set; }
    public List<int> color { get; set; }
    public List<int> rims { get; set; }
    public List<string> packs { get; set; }

    public CarConfig(string version, string desc, List<string> drive, List<int> color, List<int> rims, List<string> packs){
		this.version = version;
		this.desc = desc;
		this.drive = drive;
		this.color = color;
		this.rims = rims;
		this.packs = packs;
    }
}