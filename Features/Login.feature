Feature:Login
    As a Saucedemo user
    I want to log in with valid credentials. 
    So that I can access the store 
    
    Scenario: successful login with a valid user
        Given I am on a Saucedemo login page
        When I log in with user name "standard_user" And password "secret_sauce"  
        Then I should see the inventory page. 