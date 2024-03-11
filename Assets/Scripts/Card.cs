using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Card Class that is on each individual Card.
//Cards have a value and suite assigned to them for comparison purposes. With more time, larger projects I might've made the Suites enumerators
// for clarity rather than giving them a non intuitive integer value.

public class Card : MonoBehaviour
{
    public int value; //Value of the card, Jacks are 11, Queens are 12, Kings are 13, Aces are 14
    public int suite; //Suite of the card, Spades are 1, Clubs are 2, Diamonds are 3, Hearts are 4.


    // Function that compares two cards, returns a positive int if the card doing the comparing is greater, negative if not and 0 if they are the same card.
    public int compareCards(Card otherCard)
    {
        Debug.Log("Comparing: " + this.name + " with " + otherCard.gameObject.name);

        //Check Suites first
        if(this.suite > otherCard.suite)
        {
            return 1;
        }
        else if(this.suite < otherCard.suite)
        {
            return -1;
        }
        else 
        {
            //Check Value after suites
            if(this.value > otherCard.value)
            {
                return 1;
            }
            else if (this.value < otherCard.value)
            {
                return -1;
            }
            else 
            {
                return 0;
            }
        }
    }

}
