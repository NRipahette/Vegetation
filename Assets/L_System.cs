using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class L_System : MonoBehaviour
{
    public bool SaveAsPrefab;
    public float iterations;
    public string Alphabet;
    public Dictionary<char, string> Rules;
    public float BranchWeight; // définis si les branches ont tendances à allez vers le haut ou non
    public float EspaceBranchesPrincipale; // Bazar
    public float EspacePetiteBranche; // Bazar
    private float Rétrércissement;
    private float tRétrércissement;
    public GameObject OBJ_Tronc;
    public GameObject OBJ_Branche;
    public GameObject OBJ_BrancheFine;
    public GameObject OBJ_Feuille;
    private string finalSentence;
    private Vector3 OriginalPos;
    private Quaternion OriginalRot;
    private Stack<TransformInfo> transformStack;
    private float Scale;
    private float tScale;
    List<GameObject> instanciated;

    void Start()
    {
        instanciated = new List<GameObject>();
        OriginalPos = transform.position;
        OriginalRot = transform.rotation;
        Scale = 1;
        tScale = 2;

        Rules = new Dictionary<char, string>();
        transformStack = new Stack<TransformInfo>();
        finalSentence = Alphabet;

        Rules.Add('T', "t[+C][-C][&+C][&-C][&&+C][&&-C]t[T]");
        Rules.Add('C', "c[&BF][&&&BF][C]");
        Rules.Add('B', "b[+D][D]");
        Rules.Add('D', "d[+F][-F]");


        //Rules.Add('T', "T[+//CB][**-CCB][+CB]T");
        //Rules.Add('B', "/B[*+B[+/D[+F]]" +
        //    "[/-B[/-D[-F]]][+F][*-F]]" +
        //    "*B[/F][/+F][*-F]");
        //Rules.Add('C', "C");
        //Rules.Add('D', "*D[+F][-F]");

        Rétrércissement = 1.1f - 1 / iterations;
        tRétrércissement = 1 / iterations;
        for (int i = 0; i < iterations; i++)
        {
            finalSentence = ApplyRuleToSentence(finalSentence);
        }

        finalSentence = finalSentence.Replace("C", "");
        finalSentence = finalSentence.Replace("T", "");
        finalSentence = finalSentence.Remove(finalSentence.LastIndexOf('t'));
        RenderTree();
        foreach (GameObject item in instanciated)
        {
            item.transform.SetParent(this.transform, true);
        }
        if (SaveAsPrefab)
        {
            string localPath = AssetDatabase.GenerateUniqueAssetPath("Assets/" + gameObject.name + ".prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
        }
        // T = Tronc 
        // t = Tronc "Traité"        
        // C = Branche Principale
        // c = Branche "traitée"
        // B = Branche intermédiaire
        // b = Branche intermédiaire "traitée"
        // D = Petite Branche
        // d = Petite Branche "traitée"
        // F = Feuille
        // - +  = Rotation sur l'axe Right
        // / *  = Rotation sur l'axe Forward
        // &  = Rotation sur l'axe Up
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            finalSentence = Alphabet;
            transform.position = OriginalPos;
            transform.rotation = OriginalRot;

            foreach (var item in instanciated)
            {
                Destroy(item);
            }
            Rétrércissement = 1f / iterations;
            for (int i = 0; i < iterations; i++)
            {
                finalSentence = ApplyRuleToSentence(finalSentence);
            }

            finalSentence = finalSentence.Replace("C", "");
            finalSentence = finalSentence.Replace("T", "");
            finalSentence = finalSentence.Remove(finalSentence.LastIndexOf('t'));
            RenderTree();
            foreach (GameObject item in instanciated)
            {
                item.transform.SetParent(this.transform, true);
            }

        }
    }

    private void RenderTree()
    {
        foreach (char c in finalSentence)
        {
            switch (c)
            {
                case 'T':
                    Vector3 TPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * Scale);
                    GameObject Tronc = Instantiate(OBJ_Tronc, TPosition, transform.rotation, null);
                    Tronc.transform.localScale *= Scale;
                    instanciated.Add(Tronc);
                    break;
                case 't':
                    Vector3 tPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * Scale);
                    GameObject tronc = Instantiate(OBJ_Tronc, tPosition, transform.rotation, null);
                    tronc.transform.localScale = new Vector3(tScale, Scale, tScale);
                    tScale = tScale - tRétrércissement;
                    instanciated.Add(tronc);
                    break;
                case 'C':
                    Vector3 CPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * Scale);
                    GameObject Cbranch = Instantiate(OBJ_Branche, CPosition, transform.rotation, null);
                    Cbranch.transform.localScale *= Scale;
                    instanciated.Add(Cbranch);
                    break;
                case 'c':
                    Vector3 cPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * Scale);
                    GameObject cbranch = Instantiate(OBJ_Branche, cPosition, transform.rotation, null);
                    cbranch.transform.localScale *= Scale;
                    instanciated.Add(cbranch);
                    break;

                case 'B':
                    Vector3 BPosition = transform.position;
                    transform.Rotate(Vector3.up * UnityEngine.Random.Range(-40f, 40f));
                    transform.Rotate(Vector3.left * BranchWeight);
                    transform.Translate(Vector3.up * 2 * Scale);
                    GameObject Branch = Instantiate(OBJ_Branche, BPosition, transform.rotation, null);
                    Branch.transform.localScale *= Scale;
                    instanciated.Add(Branch);
                    break;
                case 'b':
                    Vector3 bPosition = transform.position;
                    transform.Rotate(Vector3.up * UnityEngine.Random.Range(-40f, 40f));
                    transform.Rotate(Vector3.left * BranchWeight);
                    transform.Translate(Vector3.up * 2 * Scale);
                    GameObject branch = Instantiate(OBJ_Branche, bPosition, transform.rotation, null);
                    branch.transform.localScale *= Scale;
                    instanciated.Add(branch);
                    break;
                case 'D':
                    Vector3 DPosition = transform.position;
                    transform.Rotate(Vector3.up * UnityEngine.Random.Range(-50f, 50f));
                    transform.Rotate(Vector3.left * EspacePetiteBranche);
                    transform.Translate(Vector3.up * 2 * Scale);
                    GameObject Dbranch = Instantiate(OBJ_BrancheFine, DPosition, transform.rotation, null);
                    Dbranch.transform.localScale *= Scale;
                    instanciated.Add(Dbranch);
                    break;
                case 'd':
                    Vector3 dPosition = transform.position;
                    transform.Rotate(Vector3.up * UnityEngine.Random.Range(-50f, 50f));
                    transform.Rotate(Vector3.left * EspacePetiteBranche);
                    transform.Translate(Vector3.up * 2 * Scale);
                    GameObject dbranch = Instantiate(OBJ_BrancheFine, dPosition, transform.rotation, null);
                    dbranch.transform.localScale *= Scale;
                    instanciated.Add(dbranch);
                    break;
                case 'F':
                    Vector3 FPosition = transform.position;
                    transform.Translate(Vector3.up * 2 * Scale);
                    GameObject Fbranch = Instantiate(OBJ_Feuille, FPosition, transform.rotation, null);
                    Fbranch.transform.localScale *= Scale;
                    instanciated.Add(Fbranch);
                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.right * BranchWeight);
                    break;

                case '-':
                    transform.Rotate(Vector3.left * BranchWeight);
                    break;


                case '*':
                    transform.Rotate(Vector3.forward * EspaceBranchesPrincipale);
                    break;

                case '/':
                    transform.Rotate(Vector3.back * EspaceBranchesPrincipale);
                    break;

                case '&':
                    transform.Rotate(Vector3.up * EspaceBranchesPrincipale);
                    break;


                case '[':
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation,
                        scale = Scale
                    });
                    Scale = Scale * Rétrércissement;
                    break;

                case ']':
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    Scale = ti.scale;
                    break;

            }

            //if ( c == 'B')
            //if ( c == 'C')
            //if ( c == 'D')
            //if ( c == 'F')
        }
    }

    private string ApplyRuleToSentence(string phrase)
    {
        string newPhrase = "";
        foreach (char c in phrase)
        {
            newPhrase += this.FindMatchingRule(c);
        }

        return newPhrase;
    }

    private string FindMatchingRule(char c_)
    {
        foreach (KeyValuePair<char, string> myRule in Rules)
        {
            if (myRule.Key == c_)
            {
                return myRule.Value;
            }
        }
        return c_.ToString();
    }
}
