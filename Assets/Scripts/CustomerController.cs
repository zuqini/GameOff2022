using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private float timeElapsed = Mathf.Infinity;
    private float colorTimeElapsed = Mathf.Infinity;
    private bool positionInitialized = false;
    private bool dialogueInitialized = false;
    private Order order;

    private List<string> greetings = new List<string>();
    private List<string> blacktea = new List<string>();
    private List<string> herbaltea = new List<string>();
    private List<string> lighttea = new List<string>();
    private List<string> lightStr = new List<string>();
    private List<string> mediumStr = new List<string>();
    private List<string> heavyStr = new List<string>();
    private List<string> additives = new List<string>();
    private List<string> honey = new List<string>();
    private List<string> sugar = new List<string>();
    private List<string> milk = new List<string>();
    private List<string> oatmilk = new List<string>();
    private List<string> spoon = new List<string>();
    private List<string> ender = new List<string>();

    public float lerpDuration = 1f;
    public float colorLerpDuration = 1f;
    public Transform startPosition;
    public Transform targetPosition;
    public TriggerTextBubbleController triggerTextBubble;
    public string customerName = "Jane Doe";

    public void Init()
    {
        sprite.color = Color.black;
        timeElapsed = 0;

        order = Order.RandomBasic();

    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        InitDialogue();
        Init();
    }

    void InitDialogue()
    {
        greetings.Add("Hey there!\n");
        greetings.Add("Hello~\n");
        greetings.Add("Hiya!\n");
        greetings.Add("Howdy!\n");
        greetings.Add("Good day!\n");
        greetings.Add("Wassup!\n");

        blacktea.Add("Can I get a cup of black tea? ");
        herbaltea.Add("Can I get a cup of herbal tea? ");
        lighttea.Add("Can I get a cup of light tea? ");

        lightStr.Add("Just one tea bag is fine.\n");
        mediumStr.Add("Make it a teeny bit stronger than usual.\n");
        heavyStr.Add("Make it strong for me!\n");

        additives.Add("Can I also get ");
        honey.Add("a bit of honey");
        sugar.Add("{0} sugar cubes");
        milk.Add("a splash of milk");
        oatmilk.Add("a splash of oatmilk");
        spoon.Add("a small stir spoon");

        ender.Add("Thanks.");
        ender.Add("Thanks a bunch!");
        ender.Add("Really appreciate it!");
    }

    void FixedUpdate()
    {
        if (timeElapsed < lerpDuration)
        {
            var dt = Mathfx.Hermite(0, 1, timeElapsed / lerpDuration);
            var targetPos = Vector3.Lerp(startPosition.position, targetPosition.position, dt);
            transform.position = targetPos;

            timeElapsed += Time.deltaTime;
        } else if (!positionInitialized) {
            transform.position = targetPosition.position;
            colorTimeElapsed = 0;
            positionInitialized = true;
        }

        if (colorTimeElapsed < colorLerpDuration)
        {
            var dt = Mathfx.Hermite(0, 1, colorTimeElapsed / colorLerpDuration);
            sprite.color = Color.Lerp(Color.black, Color.white, dt);

            colorTimeElapsed += Time.deltaTime;
        } else if (positionInitialized && !dialogueInitialized) {
            sprite.color = Color.white;
            triggerTextBubble.Spawn();
            dialogueInitialized = true;
        }
    }

    public Dialogue GenerateOrderDialogueByText()
    {
        var dialogue = new List<string>();
        dialogue.Add(GetRandomDialogueLine(greetings));
        if (order.blackTea > 0) {
            dialogue.Add(GetRandomDialogueLine(blacktea));
            dialogue.Add(GetRandomStrengthDialogueLine(order.blackTea));
        } else if (order.herbTea > 0) {
            dialogue.Add(GetRandomDialogueLine(herbaltea));
            dialogue.Add(GetRandomStrengthDialogueLine(order.herbTea));
        } else if (order.lightTea > 0) {
            dialogue.Add(GetRandomDialogueLine(lighttea));
            dialogue.Add(GetRandomStrengthDialogueLine(order.lightTea));
        }

        int numItems = (order.hasHoney ? 1 : 0) + 
            (order.hasMilk ? 1 : 0) + 
            (order.hasOatMilk ? 1 : 0) + 
            (order.hasSpoon ? 1 : 0) + 
            (order.sugarCount > 0 ? 1 : 0);

        if (numItems > 0)
        {
            int numCurrentItems = 0;
            dialogue.Add(GetRandomDialogueLine(additives));
            if (order.hasHoney) {
                dialogue.Add(GetRandomDialogueLine(honey));
                numCurrentItems++;
                AddConjunction(dialogue, numCurrentItems, numItems);
            }
            if (order.sugarCount > 0) {
                dialogue.Add(string.Format(GetRandomDialogueLine(sugar), order.sugarCount));
                numCurrentItems++;
                AddConjunction(dialogue, numCurrentItems, numItems);
            }
            if (order.hasMilk) {
                dialogue.Add(GetRandomDialogueLine(milk));
                numCurrentItems++;
                AddConjunction(dialogue, numCurrentItems, numItems);
            }
            if (order.hasOatMilk) {
                dialogue.Add(GetRandomDialogueLine(oatmilk));
                numCurrentItems++;
                AddConjunction(dialogue, numCurrentItems, numItems);
            }
            if (order.hasSpoon) {
                dialogue.Add(GetRandomDialogueLine(spoon));
                numCurrentItems++;
            }
            dialogue.Add(".\n");
        }

        dialogue.Add(GetRandomDialogueLine(ender));
        return new Dialogue {
            name = customerName,
            sentences = new string[] { string.Join("", dialogue) },
        };
    }

    private void AddConjunction(List<string> dialogue, int numCurrentItems, int numItems)
    {
        if (numCurrentItems == 0) return;
        else if (numCurrentItems < numItems - 1) dialogue.Add(", ");
        else if (numCurrentItems < numItems) dialogue.Add(", and ");
    }

    private string GetRandomDialogueLine(List<string> dialogue)
    {
        return dialogue[Utils.Random.Next(dialogue.Count)];
    }

    private string GetRandomStrengthDialogueLine(int strength)
    {
        switch(strength)
        {
            case 1:
                return lightStr[Utils.Random.Next(lightStr.Count)];
            case 2:
                return mediumStr[Utils.Random.Next(mediumStr.Count)];
            case 3:
                return heavyStr[Utils.Random.Next(heavyStr.Count)];
            default:
                Debug.Log("impossible strength " + strength);
                return "";
        }
    }
}
