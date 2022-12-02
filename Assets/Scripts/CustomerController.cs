using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private AudioSource audioSource;
    private string customerName = "Jane Doe";
    private float timeElapsed = Mathf.Infinity;
    private float colorTimeElapsed = Mathf.Infinity;
    private bool firstPhase = false;
    private bool secondPhase = false;
    private bool hasOrdered = false;
    private bool exiting = false;
    private Order order;

    // @TODO: for the love of god refactor this shit
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
    private List<string> enderender = new List<string>();

    private List<string> happyOrder = new List<string>();
    private List<string> sadOrder = new List<string>();
    private List<string> sadOrderEnder = new List<string>();

    public AudioClip success;
    public AudioClip failure;
    public AudioClip drumroll;
    public Sprite[] maleSpriteArray;
    public Sprite[] femaleSpriteArray;
    public string[] maleNames;
    public string[] femaleNames;
    public float lerpDuration = 1f;
    public float colorLerpDuration = 1f;
    public Transform startPosition;
    public Transform targetPosition;
    public TriggerTextBubbleController triggerTextBubble;

    public bool HasOrdered { get => hasOrdered; }
    public Order Order { get => order; }

    // @TODO: refactor this
    private void PickNewCharacter()
    {
        var character = Utils.Random.Next(8);
        var gender = Utils.Random.Next(2);
        if (gender == 0) {
            if (femaleNames[character] == customerName) {
                PickNewCharacter();
                return;
            }
            sprite.sprite = femaleSpriteArray[character];
            customerName =  femaleNames[character];
        } else {
            if (maleNames[character] == customerName) {
                PickNewCharacter();
                return;
            }
            sprite.sprite = maleSpriteArray[character];
            customerName =  maleNames[character];
        }
    }

    public void Init()
    {
        PickNewCharacter();
        exiting =  false;
        sprite.color = Color.black;
        timeElapsed = 0;
        firstPhase = false;
        secondPhase = false;
        hasOrdered = false;
        exiting = false;

        order = Order.RandomBasic();
    }

    public void Exit()
    {
        firstPhase = false;
        secondPhase = false;
        exiting = true;
        sprite.color = Color.white;
        colorTimeElapsed = 0;
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        InitDialogue();
        Init();
    }

    public void PlayDrumRoll()
    {
        audioSource.PlayOneShot(drumroll);
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
        heavyStr.Add("Make it really really strong for me!\n");

        additives.Add("Can I also get ");
        honey.Add("a bit of honey");
        sugar.Add("{0} sugar cube");
        milk.Add("a splash of milk");
        oatmilk.Add("a splash of oatmilk");
        spoon.Add("a small stir spoon");

        ender.Add("Thanks.");
        ender.Add("Thanks a bunch!");
        ender.Add("Really appreciate it!");

        enderender.Add("Ummm.. Did you get all that?");
        enderender.Add("I'll be waiting here then!");
        enderender.Add("Hope you won't neen me to repeat that!");
        enderender.Add("Can't wait!");

        happyOrder.Add("This is just perfect. I love it!");
        happyOrder.Add("Wow, you didn't screw it up like Starpucks did. Awesome!");
        happyOrder.Add("I'm totally gonna rate this place 5 stars on Coocle.com!");

        sadOrder.Add("Ewww! What is this?\n");
        sadOrder.Add("What the heck!\n");
        sadOrder.Add("Umm, this is not what I asked for..\n");

        sadOrderEnder.Add("I'm sad...");
        sadOrderEnder.Add("I'm only gonna go to Starpucks from now on.");
        sadOrderEnder.Add("I'm gonna rate this place 0 stars on Coocle.com...");
    }

    void FixedUpdate()
    {
        if (exiting)
        {
            if (colorTimeElapsed < colorLerpDuration)
            {
                var dt = Mathfx.Hermite(0, 1, colorTimeElapsed / colorLerpDuration);
                sprite.color = Color.Lerp(Color.white, Color.black, dt);

                colorTimeElapsed += Time.deltaTime;
            } else if (!firstPhase) {
                sprite.color = Color.black;
                timeElapsed = 0;
                firstPhase = true;
            }

            if (timeElapsed < lerpDuration)
            {
                var dt = Mathfx.Hermite(0, 1, timeElapsed / lerpDuration);
                var targetPos = Vector3.Lerp(targetPosition.position, startPosition.position, dt);
                transform.position = targetPos;

                timeElapsed += Time.deltaTime;
            } else if (firstPhase && !secondPhase) {
                transform.position = startPosition.position;
                secondPhase = true;
                Init();
            }
        } else {
            if (timeElapsed < lerpDuration)
            {
                var dt = Mathfx.Hermite(0, 1, timeElapsed / lerpDuration);
                var targetPos = Vector3.Lerp(startPosition.position, targetPosition.position, dt);
                transform.position = targetPos;

                timeElapsed += Time.deltaTime;
            } else if (!firstPhase) {
                transform.position = targetPosition.position;
                colorTimeElapsed = 0;
                firstPhase = true;
            }

            if (colorTimeElapsed < colorLerpDuration)
            {
                var dt = Mathfx.Hermite(0, 1, colorTimeElapsed / colorLerpDuration);
                sprite.color = Color.Lerp(Color.black, Color.white, dt);

                colorTimeElapsed += Time.deltaTime;
            } else if (firstPhase && !secondPhase) {
                sprite.color = Color.white;
                triggerTextBubble.Spawn();
                secondPhase = true;
            }
        }
    }

    public Dialogue GenerateOrderComparisonText(Order cupOrder)
    {
        hasOrdered = true;
        var dialogue = new List<string>();
        if (cupOrder.Equals(order))
        {
            audioSource.PlayOneShot(success);
            dialogue.Add(GetRandomDialogueLine(happyOrder));
        }
        else
        {
            audioSource.PlayOneShot(failure);
            dialogue.Add(GetRandomDialogueLine(sadOrder));

            if (order.blackTea != cupOrder.blackTea)
            {
                dialogue.Add(string.Format("I wanted {0} black teabag{1}, but you gave me {2}.\n", order.blackTea, order.blackTea > 1 ? "s" : "", cupOrder.blackTea == 0 ? "none" : cupOrder.blackTea));
            }
            if (order.herbTea != cupOrder.herbTea)
            {
                dialogue.Add(string.Format("I wanted {0} herbal teabag{1}, but you gave me {2}.\n", order.herbTea, order.herbTea > 1 ? "s" : "", cupOrder.herbTea == 0 ? "none" : cupOrder.herbTea));
            }
            if (order.lightTea != cupOrder.lightTea)
            {
                dialogue.Add(string.Format("I wanted {0} light teabag{1}, but you gave me {2}.\n", order.lightTea, order.lightTea > 1 ? "s" : "", cupOrder.lightTea == 0 ? "none" : cupOrder.lightTea));
            }
            if (order.hasHoney != cupOrder.hasHoney)
            {
                dialogue.Add(string.Format("I wanted {0}honey, but you gave me {1}.\n", order.hasHoney ? " " : "no " , cupOrder.hasHoney ? "honey" : "none"));
            }
            if (order.sugarCount != cupOrder.sugarCount)
            {
                dialogue.Add(string.Format("I wanted {0} sugar cube{1}, but you gave me {2}.\n", order.sugarCount, order.sugarCount > 1  ? "s" : "", cupOrder.sugarCount == 0 ? "none" : cupOrder.sugarCount));
            }
            if (order.hasMilk != cupOrder.hasMilk)
            {
                dialogue.Add(string.Format("I wanted {0}milk, but you gave me {1}.\n", order.hasMilk ? " " : "no " , cupOrder.hasMilk ? "honey" : "none"));
            }
            if (order.hasOatMilk != cupOrder.hasOatMilk)
            {
                dialogue.Add(string.Format("I wanted {0}oatmilk, but you gave me {1}.\n", order.hasOatMilk ? " " : "no ", cupOrder.hasOatMilk ? "oatmilk" : "none"));
            }
            if (order.hasSpoon && !cupOrder.hasSpoon)
            {
                dialogue.Add(string.Format("I wanted {0}spoon, but you gave me {1}.\n", order.hasSpoon ? "a  " : "no ", cupOrder.hasOatMilk ? "one anyway" : "none"));
            }
            if (!cupOrder.hasWater)
            {
                dialogue.Add("The cup doesn't even have water in it. What do you even do here?\n");
            }
            dialogue.Add(GetRandomDialogueLine(sadOrderEnder));
        }

        return new Dialogue {
            name = customerName,
            sentences = new string[] { string.Join("", dialogue) },
        };
    }

    public List<Dialogue> GenerateOrderDialogueText()
    {
        hasOrdered = true;
        var dialogues = new List<Dialogue>();
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
                // lol timecrunch jank, @TODO refactor all this shit please
                if (order.sugarCount > 1) {
                    dialogue.Add("s");
                }
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
        // @TODO: expand canvas dinamically by string length
        return new List<Dialogue> {
            new Dialogue {
                name = customerName,
                sentences = new string[] { 
                    string.Join("", dialogue),
                    "... ",
                    "... ",
                    "... ",
                },
            },
            new Dialogue {
                name = customerName,
                sentences = new string[] { 
                    GetRandomDialogueLine(enderender),
                },
            },
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
