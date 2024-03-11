using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Big Function doing most of the work in the project. Could be broken out into multiple components.
public class InfiniteScroll : MonoBehaviour
{
    //Parameters
    public ScrollRect scroll; //Scroll Rect
    public RectTransform viewport; //Viewport Rect
    public RectTransform content; //Content Rect, also where most of the cards will be children of.
    public HorizontalLayoutGroup group; //Layout Group

    public List<RectTransform> cardList; //List holding cards, is populated in the start function.

    public Deck deck; //Instance of the deck class, used to pull random cards from. Technically the cards aren't randomly pulled but they are in
                        //no particular order.

    Vector2 past_velocity = Vector2.zero; //Variable for tracking velocity after resetting.
    bool is_updated; //Checks to see if cards were recently reset.

    //Function that pulls cards from the Deck Class and makes them children of content. Children are sorted after every card. 
    //This function could be optimized better by only sorting once all the cards are placed, but will depend on how often cards would be placed.
    private void PullCardsFromDeck()
    {
        cardList = new List<RectTransform>();

        foreach (GameObject item in deck.deckOfCards)
        {
            GameObject instance = Instantiate(item, content);
            SortChildren(instance.GetComponent<Card>());
        }
    }

    //Function that sorts the children by using the compare cards function built into the Card class.
    //Cards are ordered based on their sibling index.
    //Brute Force Sort is implemented here, sort could be improved using an altered binary search sort since the children are sorted before
    // a new card is placed.
    private void SortChildren(Card currentCard)
    {
        if(content.transform.childCount == 1)
        {
            return;
        }

        for (int i = 0; i < content.transform.childCount; i++)
        {
            Card otherCard = content.transform.GetChild(i).GetComponent<Card>();
            int comparison = currentCard.compareCards(otherCard);
            if(comparison >= 0)
            {
                currentCard.transform.SetSiblingIndex(i);
                return;
            }
        }   

        //Set as the last sibling if it isn't larger than any other card.
        currentCard.transform.SetAsLastSibling();

    }

    // Start is called before the first frame update
    void Start()
    {
        //Pull the cards from the deck.
        PullCardsFromDeck();

        //Populate cardlist now that the cards are finished sorting.
        for (int i = 0; i < content.childCount; i++)
        {
            cardList.Add(content.GetChild(i).GetComponent<RectTransform>());
        }

        //Initialize Variables.
        past_velocity = Vector2.zero;
        is_updated = false;

        //Know how many clones we need.
        int clones = cardList.Count;

        //Create Clones for the left and right side of the cards
        for (int i = 0; i < clones; i++)
        {
            //Right Side
            RectTransform left_transform = Instantiate(cardList[i % cardList.Count], content);
            left_transform.SetAsLastSibling();

            //Left Side
            int num = cardList.Count - i - 1;
            while(num < 0)
            {
                num += cardList.Count;
            }
            RectTransform right_transform = Instantiate(cardList[num], content);
            right_transform.SetAsFirstSibling();
        }
    }

    // Update is called once per frame
    void Update()
    {

        //I'll be using the update function to jump towards the middle of the cards as they are moved.

        //Keeps track of the speed at which the cards are moved.
        if(is_updated)
        {
            is_updated = false;
            scroll.velocity = past_velocity;
        }
        
        if(content.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();
            past_velocity = scroll.velocity;
            content.localPosition -= new Vector3(cardList.Count * (cardList[0].rect.width + group.spacing), 0, 0);
            is_updated = true;
        }

        if(content.localPosition.x < 0 - (cardList.Count * (cardList[0].rect.width + group.spacing)))
        {            
            Canvas.ForceUpdateCanvases();
            past_velocity = scroll.velocity;
            content.localPosition += new Vector3(cardList.Count * (cardList[0].rect.width + group.spacing), 0, 0);
            is_updated = true;
        }
        
    }
}
