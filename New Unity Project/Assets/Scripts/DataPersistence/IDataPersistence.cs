using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(OptionsData data);

    void SaveData(ref OptionsData data);
}
