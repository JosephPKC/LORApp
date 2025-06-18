# Design Layers
## Data Access
### Db
- Db will be SQLite.
### Gateway/Repo
- Connect to db.
- Access data from db and write to it.
- Caching data.
## Business Logic
### Entity Models
- Entity models that hold data and state, and nothing else.
### Use Cases
- General behavior, business rules, and use cases.
- For each page, there are multiple use cases, such as List Cards, See Card Details, Favorite Card, etc.
## Application
### Controllers
- Manipulate and change entity models.
### Mappers
- Mappers transform one data type to another.
### View Models
- Bind entity models and business logic to the UI and vice-versa.
- Transforms data between the two via mappers and controllers.
## Presentation
### Views
- Presents and organizes data into a UI.

# Structure
## Models
- Holds entity models and dtos.
## Views
- Holds the views and stylings.
## ViewModels
- Holds view models.
## Services
- Misc helpers in the application layer, including mappers and gateways.,
## UseCases
- Misc helpers and controllers in the business layer.
## Utils
- General utility classes and functions.
## Adapters
- For adapting external libraries, like sqlite and logging.
## Resources
- For static resources to load in the resource dictionary (like styles, colors, and images).

# Operations/Use Cases Per Page
## Card Listing Page
### List All Cards
- Fetch all cards from db as card listing. Card listing has less data, to avoid fetching unneeded data.
- Store the listing. This is cached fora long time, and should not be modified.
- When viewing it, create a transformation of the listing and view that instead (See Filters).
### Filters
- Filter functions transform based on flags and fields set by the filter.
- Transforms the listing copy (without changing the original), and view that.
- Use LINQ.
### Favorite
- Favoriting a card lets you add a special filter for it.
- Mark the card as favorited (or unfavorite it).
- Get the card code from the listing.
- Toggle the favorite state, and push to the db via the gateway.
- Make sure to update the cache afterwards.
### View Card
- From card listing, get the card code and card type.
- Route to the appropriate card page based on card type and the card code (See Card Page).
## Card Page
### View Card (continued)
- Load card data from card code.
- Make sure that the card data is cached in the repository.
- Display card art prominently.
- Load card data and text with LINKS.
### Favorite
- Same as favoriting in the listing,
- Update the cache for the card instead of the listing.
### See Further Info (Links)
- Links will be for things like keywords, etc.
- May just be Flyouts.
- Get the link ref code from the card page.
- Route to the appropriate fly out page based on the link ref code and what kind of link it is
## Flyout Ref Page
### View Ref
- Load ref data from ref code. Make sure it is cached in the repo (LONG).
- Display ref data as text.

# NOTES:
- To setup Property Notification, use Community Tookit for MVVM: https://blog.postsharp.net/inotifypropertychanged