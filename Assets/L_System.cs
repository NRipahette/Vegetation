using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_System : MonoBehaviour
{
    public float iterations;
    public string Alphabet;
    public Dictionary<char, string> Rules;
    public int height;
    public float Angle;
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
        //for (int i = 0; i < height; i++)
        //{
        //    Alphabet += "T";
        //}
        finalSentence = Alphabet;

        //Rules.Add('T', "t[+B][-B][&+B][&-B]tT");
        //Rules.Add('t', "t");
        //Rules.Add('B', "b[&DF][&&&DF]B");
        //Rules.Add('b', "b");
        //Rules.Add('C', "C");
        //Rules.Add('D', "d[+F][-F]");

        Rules.Add('T', "t[+C][-C][&+C][&-C][&&+C][&&-C]t[T]");
        Rules.Add('t', "t");
        Rules.Add('C', "c[&BF][&&&BF][C]");
        Rules.Add('B', "b[+D][D]");
        Rules.Add('b', "b");
        Rules.Add('D', "d[+F][-F]");


        //Rules.Add('T', "T[+//CB][**-CCB][+CB]T");
        //Rules.Add('B', "/B[*+B[+/D[+F]]" +
        //    "[/-B[/-D[-F]]][+F][*-F]]" +
        //    "*B[/F][/+F][*-F]");
        //Rules.Add('C', "C");
        //Rules.Add('D', "*D[+F][-F]");

        Rétrércissement =1.1f -  1 / iterations;
        tRétrércissement = 1 / iterations;
        for (int i = 0; i < iterations; i++)
        {
            finalSentence = ApplyRuleToSentence(finalSentence);
        }

        finalSentence = finalSentence.Replace("C","");
        finalSentence = finalSentence.Replace("T", "");
        finalSentence = finalSentence.Remove(finalSentence.LastIndexOf('t'));
        RenderTree();

        // T = Tronc 
        // t = Tronc nue
        // F = Feuille
        // B = Branche Principale
        // C = Branche nue
        // D = Petite Branche
        // - & +  = Changement d'angle
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Alphabet = "";
            //for (int i = 0; i < height; i++)
            //{
            //    Alphabet += "T";
            //}
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

            RenderTree();

        }
    }

    private void RenderTree()
    {
        foreach (char c in finalSentence)
        {
            //rotation sur Y
            if (EspaceBranchesPrincipale > 0.01f)
                // transform.rotation = Quaternion.Euler(transform.eulerAngles.x, UnityEngine.Random.Range(0 - 180, 0 + 180) * EspaceBranchesPrincipale, transform.eulerAngles.z); // Rotation sur lui même pour que les branchent partent dans tous les sens 
                //transform.Rotate(Vector3.up * UnityEngine.Random.Range(0 - 180, 0 + 180) * RandCoef); // Rotation sur lui même pour que les branchent partent dans tous les sens 

                //Rotation sur z
                //if (ForwardCoef > 0.01f)
                //    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, UnityEngine.Random.Range(-45, 45) * ForwardCoef);
                //transform.Rotate(Vector3.forward * UnityEngine.Random.Range(-360, 360) * ForwardCoef);
                //if (transform.rotation.z > 45)
                //   transform.rotation =  Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y ,45);


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
                        tronc.transform.localScale = new Vector3( tScale, Scale, tScale);
                        tScale = tScale - tRétrércissement;
                        instanciated.Add(tronc);
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
                    case 'c':
                        Vector3 cPosition = transform.position;
                        transform.Translate(Vector3.up * 2 * Scale);
                        GameObject cbranch = Instantiate(OBJ_Branche, cPosition, transform.rotation, null);
                        cbranch.transform.localScale *= Scale;
                        instanciated.Add(cbranch);
                        break;
                    case 'C':
                        Vector3 CPosition = transform.position;
                        transform.Translate(Vector3.up * 2 * Scale);
                        GameObject Cbranch = Instantiate(OBJ_Branche, CPosition, transform.rotation, null);
                        Cbranch.transform.localScale *= Scale;
                        instanciated.Add(Cbranch);
                        break;

                    case 'D':
                        Vector3 DPosition = transform.position;
                        transform.Rotate(Vector3.up * UnityEngine.Random.Range(-50f,50f));
                        transform.Rotate(Vector3.left * EspacePetiteBranche);
                        transform.Translate(Vector3.up * 2 * Scale);
                        GameObject Dbranch = Instantiate(OBJ_BrancheFine, DPosition, transform.rotation, null);
                        Dbranch.transform.localScale *= Scale;
                        instanciated.Add(Dbranch);
                        break;
                    case 'd':
                        Vector3 dPosition = transform.position;
                        transform.Rotate(Vector3.up * UnityEngine.Random.Range(-50f,50f));
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
                        //transform.Rotate(Vector3.right * UnityEngine.Random.Range(BranchWeight + Angle - (Angle / 3), BranchWeight + Angle + (Angle / 3)));
                        break;

                    case '-':
                        transform.Rotate(Vector3.left * BranchWeight);

                        //transform.Rotate(Vector3.left * UnityEngine.Random.Range(BranchWeight + Angle - (Angle / 3), BranchWeight + Angle + (Angle / 3)));
                        break;


                    case '*':
                        transform.Rotate(Vector3.forward * EspaceBranchesPrincipale);
                        //transform.Rotate(Vector3.up * (130 + UnityEngine.Random.Range(-20, 40) * ForwardCoef));
                        break;

                    case '/':
                        transform.Rotate(Vector3.back * EspaceBranchesPrincipale);
                        //transform.Rotate(Vector3.down * (130 + UnityEngine.Random.Range(-20, 40) * ForwardCoef));
                        break;

                    case '&':
                        transform.Rotate(Vector3.up * EspaceBranchesPrincipale);
                        //transform.Rotate(Vector3.up * (130 + UnityEngine.Random.Range(-20, 40) * ForwardCoef));
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
