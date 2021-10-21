using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_System : MonoBehaviour
{
    public int iterations;
    public string Alphabet;
    public Dictionary<char, string> Rules;
    public float Angle;
    public float BranchWeight; // définis si les branches ont tendances à allez vers le haut ou non
    public float UpCoef; // Bazar
    public float ForwardCoef; // Bazar
    public GameObject OBJ_Tronc;
    public GameObject OBJ_Branche;
    public GameObject OBJ_BrancheFine;
    public GameObject OBJ_Feuille;
    private string finalSentence;
    private Vector3 OriginalPos;
    private Quaternion OriginalRot;
    Vector3 Pos;
    private Stack<TransformInfo> transformStack;
    List<GameObject> instanciated;

    void Start()
    {
        instanciated = new List<GameObject>();
        OriginalPos = transform.position;
        OriginalRot = transform.rotation;

        Rules = new Dictionary<char, string>();
        transformStack = new Stack<TransformInfo>();
        finalSentence = Alphabet;

        Rules.Add('T', "T[+CB][-CB][+CB]T");
        Rules.Add('B', "B[+B[+D[++F]]" +
            "[-B[-D[-F]]][++F][--F]]" +
            "B[F][++F][--F]");
        Rules.Add('C', "C");
        Rules.Add('D', "D[+F][-F]");

        for (int i = 0; i < iterations; i++)
        {
            finalSentence = ApplyRuleToSentence(finalSentence);
        }

        RenderTree();

        // T = Tronc 
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
            finalSentence = Alphabet;
            transform.position = OriginalPos;
            transform.rotation = OriginalRot;

            foreach (var item in instanciated)
            {
                Destroy(item);
            }
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

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, UnityEngine.Random.Range(0 - 180, 0 + 180) * UpCoef, transform.eulerAngles.z); // Rotation sur lui même pour que les branchent partent dans tous les sens 
            //transform.Rotate(Vector3.up * UnityEngine.Random.Range(0 - 180, 0 + 180) * RandCoef); // Rotation sur lui même pour que les branchent partent dans tous les sens 
            transform.Rotate(Vector3.forward * UnityEngine.Random.Range(-360, 360) * ForwardCoef);
            if (transform.rotation.z > 45)
                transform.rotation =  Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y ,45);
           // transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y , UnityEngine.Random.Range( -45, 45) * ForwardCoef);

            switch (c)
            {
                case 'T':
                    Vector3 TPosition = transform.position;
                    transform.Translate(Vector3.up * 2);
                    transform.Rotate(Vector3.forward * UnityEngine.Random.Range(-360, 360) * ForwardCoef);
                    instanciated.Add(Instantiate(OBJ_Tronc, TPosition, transform.rotation, null));
                    break;
                case 'B':
                    Vector3 BPosition = transform.position;
                    transform.Translate(Vector3.up * 2);

                    instanciated.Add(Instantiate(OBJ_Branche, BPosition, transform.rotation, null));
                    break;
                case 'C':
                    Vector3 CPosition = transform.position;
                    transform.Translate(Vector3.up * 2);

                    instanciated.Add(Instantiate(OBJ_Branche, CPosition, transform.rotation, null));
                    break;

                case 'D':
                    Vector3 DPosition = transform.position;
                    transform.Translate(Vector3.up * 2);

                    instanciated.Add(Instantiate(OBJ_BrancheFine, DPosition, transform.rotation, null));
                    break;
                case 'F':
                    Vector3 FPosition = transform.position;
                    transform.Translate(Vector3.up * 2);

                    instanciated.Add(Instantiate(OBJ_Feuille, FPosition, transform.rotation, null));
                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.right * UnityEngine.Random.Range(BranchWeight + Angle - (Angle / 3), BranchWeight + Angle + (Angle / 3)));
                    //transform.Rotate(Vector3.forward * UnityEngine.Random.Range(-360, 360) * ForwardCoef);


                    break;

                case '-':
                    transform.Rotate(Vector3.left * UnityEngine.Random.Range(BranchWeight + Angle - (Angle / 3), BranchWeight + Angle + (Angle / 3)));
                    //transform.Rotate(Vector3.forward * UnityEngine.Random.Range(-360, 360) * ForwardCoef);
                    break;

                case '[':
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']':
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
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
