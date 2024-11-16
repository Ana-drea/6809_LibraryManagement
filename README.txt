How to run:
Open the terminal under LibraryManagement directory. Run "dotnet run" command, the project should be running on the localhost port that appears on terminal output.

Click on "Book", "Author", "Customer", "Branch" tabs in the navbar to perform CRUD operations on the corresponding data table.

Note that to add a new book, first make sure you've added corresponding author and library branch under the Author and Branch tab; The removal of any author or library branch will also result in the removal of all the books written by that author/stored in that branch.

Links to login, logout, register as well as Manage account pages are added on the navbar. Login first and then click on the little human icon to the top right of the navbar to access Manage account pages.

Book, Author, Customer and Branch pages are authenticated, access to these pages requires user login.

Click on "Google" or "Facebook" button on the login page to use social media authentication. Follow the instructions and the website will redirect you back to the register page once you've pass the authentication.

Swagger API is integrated as another way to perform CRUD operations to Customer. Navigate to https://localhost:<port>/index.html and perform Gost, Put and Delete methods on Customer, and the result will immediately show in https://localhost:<port>/Customer/Details page