using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;




namespace FOV_changer
{
    [BepInPlugin("Vecter.FOV.Mod", "FOV Mod", "1.0.0")]
    [BepInProcess("Vecter.exe")]
    public class FOV_Mod : BaseUnityPlugin
    {
        public bool fileFuse;
        public bool isEnabled;
        public bool keyDepressed;
        public string fovSettings;
        public string fovYN;
        public int fovIndex;
        public OptionsMenu2 menu;
        void Start()
        {
            SaveFiles();
            if(File.Exists(Application.persistentDataPath + @"/AwkwardModSaves/FOVchanger.moddata") && File.ReadAllText(Application.persistentDataPath + @"/AwkwardModSaves/FOVchanger.moddata") != "")
            {
                LoadAll();
            }
            BepVecterModCore.VecterModCore.ModSettings.Add("- Custom FOV");
            BepVecterModCore.VecterModCore.ModSettings.Add("- Enable FOV");
            Logger.Log(BepInEx.Logging.LogLevel.Message, "Fov Mod Active");
            cprt.fieldOfView = fov;
        }

        void Update()
        {
            OptionsChanges();
            DataToSave();
            if (Input.GetKey(KeyCode.Return))
            {
                SaveAll();
            }
            
                
            ChangeFov();
        }
        void LateUpdate()
        {
            ChangeFov();
        }
        void FixedUpdate()
        {
            ChangeFov();
        }
        CameraProjectionRenderingToolkit.CPRT cprt;
        float fov;
        void ChangeFov()
        {
            cprt = Resources.FindObjectsOfTypeAll<CameraProjectionRenderingToolkit.CPRT>().First();
            if (isEnabled)
            {
                cprt.fieldOfView = fov;
            }
            else
            {
                cprt.fieldOfView = 60;
            }
            
            
            Logger.Log(BepInEx.Logging.LogLevel.Message, Camera.current.fieldOfView.ToString());
            switch (fovIndex)
            {
                case 1:
                    fov = 20f;
                    break;
                case 2:
                    fov = 40f;
                    break;

                case 3:
                    fov = 60f;
                    
                    break;
                case 4:
                    fov = 90f;
                    break;
                case 5:
                    fov = 100f;
                    break;
                 
            }
        }





        void OptionsChanges()
        {
            menu = Resources.FindObjectsOfTypeAll<OptionsMenu2>().FirstOrDefault();
            if(menu.SelectedOption == "Enable FOV")
            {
                menu.SelectedOption += "\n\nIf this option is disabled, the FOV wont actually change.\n(disabled on default)\n" + fovYN;
                if(Input.GetKeyDown(KeyCode.Return) && !keyDepressed)
                {
                    if (isEnabled)
                    {
                        isEnabled = false;
                    }
                    else
                    {
                        isEnabled = true;
                    }
                    keyDepressed = true;

                }
                if (Input.GetKeyUp(KeyCode.Return)) { keyDepressed = false; }
            }
            if (isEnabled)
            {
                fovYN = " [ enabled ] /   disabled   ";
            }
            else
            {
                fovYN = "   enabled   / [ disabled ] ";
            }


            if(menu.SelectedOption == "Custom FOV")
            {
                menu.SelectedOption += "\n\nHave you ever wanted to see farther, but didn't know how? Well here you go!\n\n" + fovSettings;

                if(Input.GetKeyDown(KeyCode.RightArrow) && !keyDepressed)
                {
                    fovIndex += 1;
                    keyDepressed = true;
                }
                if(Input.GetKeyDown(KeyCode.LeftArrow) && !keyDepressed)
                {
                    fovIndex -= 1;
                }
                if(Input.GetKeyDown(KeyCode.Return) && !keyDepressed)
                {
                    fovIndex += 1;
                }
                
            }
                if (Input.GetKeyUp(KeyCode.LeftArrow)) { keyDepressed = false; }
                if (Input.GetKeyUp(KeyCode.RightArrow)) { keyDepressed = false; }
                if (Input.GetKeyUp(KeyCode.Return)) { keyDepressed = false; }
            switch (fovIndex)
            {
                case 0:
                    fovIndex = 5;
                    break;
                case 1:
                    fovSettings = "[ low ]   med     default     high   \n  quake pro  ";
                    break;
                case 2:
                    fovSettings = "  low   [ med ]   default     high   \n  quake pro  ";
                    break;
                case 3:
                    fovSettings = "  low     med   [ default ]   high   \n  quake pro  ";
                    break;
                case 4:
                    fovSettings = "  low     med     default   [ high ] \n  quake pro  ";
                    break;
                case 5:
                    fovSettings = "  low     med     default     high   \n[ quake pro ]";
                    break;
                case 6:
                    fovIndex = 1;
                    break;
            }
        }

        void SaveFiles()
        {
            fileFuse = true;
            if (!Directory.Exists(Application.persistentDataPath + @"/AwkwardModSaves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + @"/AwkwardModSaves");
            }
            if (!File.Exists(Application.persistentDataPath + @"/AwkwardModSaves/FOVchanger.moddata"))
            {
                File.Create(Application.persistentDataPath + @"/AwkwardModSaves/FOVchanger.moddata");
            }
            fileFuse = false;
        }

        void SaveAll()
        {
            if(!File.Exists(Application.persistentDataPath + @"/AwkwardModSaves/FOVchanger.moddata"))
            {
                File.Create(Application.persistentDataPath + @"/AwkwardModSaves/FOVchanger.moddata");
            }

            FileStream stream = new FileStream(Application.persistentDataPath + @"/AwkwardModSaves/FOVchanger.moddata", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, DataToSave());
            stream.Close();
        }
        public string FovIndexToSave;
        public string isEnabledToSave;
        object DataToSave()
        {
            
            switch (fovIndex)
            {
                case 1:
                    FovIndexToSave = "fovIndex=1";
                    break;
                case 2:
                    FovIndexToSave = "fovIndex=2";
                    break;
                case 3:
                    FovIndexToSave = "fovIndex=3";
                    break;
                case 4:
                    FovIndexToSave = "fovIndex=4";
                    break;
                case 5:
                    FovIndexToSave = "fovIndex=5";
                    break;
            }

            if (isEnabled)
            {
                isEnabledToSave = "enabled=true";
            }
            else
            {
                isEnabledToSave = "enabled=false";
            }

            return (FovIndexToSave, isEnabledToSave);
        }
        void LoadAll()
        {
            if(File.Exists(Application.persistentDataPath + @"/AwkwardModSaves/FOVchanger.moddata") && !fileFuse)
            {
                FileStream stream = new FileStream(Application.persistentDataPath + @"/AwkwardModSaves/FOVchanger.moddata", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                string saveString = formatter.Deserialize(stream).ToString();
                if (saveString.Contains("enabled=true"))
                {
                    isEnabled = true;
                }
                if (saveString.Contains("enabled=false"))
                {
                    isEnabled = false;
                }
                if(saveString.Contains("fovIndex=1")) { fovIndex = 1; }
                if(saveString.Contains("fovIndex=2")) { fovIndex = 2; }
                if(saveString.Contains("fovIndex=3")) { fovIndex = 3; }
                if(saveString.Contains("fovIndex=4")) { fovIndex = 4; }
                if(saveString.Contains("fovIndex=5")) { fovIndex = 5; }
                if(saveString == "")
                {
                    isEnabled = true;
                    fovIndex = 3;
                }
                stream.Close();
            }
        }

        void OnApplicationQuit()
        {
            DataToSave();
            SaveAll();
        }
    }
}
