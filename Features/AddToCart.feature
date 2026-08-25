@regression
Feature: Add to cart 

    Background:
        Given I am logged in as "standard_user"
        
    Scenario: Add a single item to the cart
        When I add "Sauce Labs Backpack" to the cart
        Then the cart should contain 1 item