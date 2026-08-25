Feature:Login
    As a Saucedemo user
    I want to log in with valid credentials. 
    So that I can access the store 
    
    @smoke
    Scenario: successful login with a valid user
        Given I am on a Saucedemo login page
        When I log in with username "standard_user" And password "secret_sauce"  
        Then I should see the inventory page
    
    @regression
    Scenario: Failed login with invalid credentials
        Given I am on a Saucedemo login page
        When I log in with username "astandard_user" And password "wrong_password"
        Then I should see an error message "@@Epic sadface: Username and password do not match any user in this service"

   

