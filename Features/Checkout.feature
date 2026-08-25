@smoke
Feature: Checkout

  Background:
    Given I am logged in as "standard_user"

  Scenario: Complete checkout with valid information
    When I add "Sauce Labs Backpack" to the cart
    And I proceed to checkout
    And I fill in checkout information with first name "John", last name "Doe", and zip code "12345"
    And I complete the checkout
    Then I should see the order confirmation "Thank you for your order!"