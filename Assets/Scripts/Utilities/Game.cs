using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartLocalization;

public class GameObjectData
{
    public string objName;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
 
}

public static class Game {


   public static List<Enemy> enemiesList;
    //Lista de todo lo que se debe guardar
   public static AsyncOperation sceneOperation = null;

   public static List<GameObjectData> transformsList;

   public static float timePlayed;
   public static bool initialized = false;

    public static bool isFading = true;

   public static bool returningFromBattle = false;


    //Variables de inicialziación de nivel

    public static bool firstSequenceDone = false;   //Se ha visto la intro inicial?
    public static bool puzzleClockFinished = false; //Se ha completado el puzle del reloj?
	public static bool puzzleBooksFinished = false; //Se ha completado el puzle del reloj?
	public static bool puzzlePasswordFinished = false; //Se ha completado el puzle del password?

    public static bool paperFromTrashObtained = false; //Se ha pillado el papel de la papelera?
    public static bool paperFromDesktopObtained = false; //Se ha pillado el papel del escritorio?
    public static bool paperFromTeddyObtained = false; //Se ha pillado el papel del osito?


    public static bool chestEmpty = false; //Cofre vacio?
    public static bool boyAppeared = false; //Aparece el chico
	public 	static bool conversationAboutClockDone = false; //Hablaron del dispositivo de introducción de coordenadas camuflado?


	public static Vector3 libraryFinalPosition = new Vector3(1.78f, 0.95f, 0);
    public static Vector3 chestPosition = Vector3.zero;

    public static bool sockoAppears = false;
    public static bool sockoDefeated = false;

    public static bool finalBossDefeated = false;


    public static int[] code = new int[3]; //0 => Papeles, 1 => Reloj, 2 => Socko

	public static int bgWindowColor = -1;
	public static int puzzlesSolved = 0;

	public static int piecesOfNumber = 0;

    public static bool alreadyEntered = false;


    public static void Init()
    {
        if(transformsList == null)
            transformsList = new List<GameObjectData>();

        CharacterParty.Init();

        //Inicializar el código de salida
        for (int i = 0; i< code.Length; ++i)
        {
            code[i] = Random.Range(0, 10);
			Debug.Log ("Code "+i+": " +code [i]);
        }

        initialized = true;
    }

    public static void AddObjectStatus(GameObjectData tempGameObject)
    {
        int index = transformsList.FindIndex(p => p.objName.Equals(tempGameObject.objName));
        if(index == -1)
        {
            transformsList.Add(tempGameObject);
        }
        else
        {
            transformsList.RemoveAt(index);
            transformsList.Add(tempGameObject);
        }
    }

    public static GameObjectData LoadSceneObjectTransformByName(string name)
    {
        return transformsList.Find(p => p.objName.Equals(name));
    }

    public static float GetPlayedTime()
    {
        return Time.timeSinceLevelLoad;
    }

    public static void ResetGame()
    {
        //enemiesList.Clear();
   
        sceneOperation = null;

        transformsList.Clear() ;

        timePlayed = 0;
        initialized = false;

        isFading = true;

        returningFromBattle = false;


        //Variables de inicialziación de nivel

        firstSequenceDone = false;   //Se ha visto la intro inicial?
        puzzleClockFinished = false; //Se ha completado el puzle del reloj?
        puzzleBooksFinished = false; //Se ha completado el puzle del reloj?
        puzzlePasswordFinished = false; //Se ha completado el puzle del password?

        paperFromTrashObtained = false; //Se ha pillado el papel de la papelera?
        paperFromDesktopObtained = false; //Se ha pillado el papel del escritorio?
        paperFromTeddyObtained = false; //Se ha pillado el papel del osito?


        chestEmpty = false; //Cofre vacio?
        boyAppeared = false; //Aparece el chico
        conversationAboutClockDone = false; //Hablaron del dispositivo de introducción de coordenadas camuflado?


        libraryFinalPosition = new Vector3(1.78f, 0.95f, 0);
        chestPosition = Vector3.zero;

        sockoAppears = false;
        sockoDefeated = false;

        finalBossDefeated = false;


        code.Initialize(); //0 => Papeles, 1 => Reloj, 2 => Socko

        bgWindowColor = -1;
        puzzlesSolved = 0;

        piecesOfNumber = 0;

        alreadyEntered = false;
    //Reset de items
        ItemsList.equipableItems.Clear();
        ItemsList.items.Clear();
    }
}
