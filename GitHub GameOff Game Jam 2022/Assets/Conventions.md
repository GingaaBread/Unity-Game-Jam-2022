**Conventions & Style Guides**

1. Preface
    This style guide is designed to keep the code written by different developers as similar as possible and avoid confusion. Version: 3
2. Classes
    1. Signatures
        - Every class should be prefaced with a /// `<author>` comment, which contains your name.
        This is done to know who to address in case there are any questions about the code
        without having to consult git commits first
    2. Class Comment
        - Every class should be prefaced with a /// `<summary>` comment, which outlines what
        the class is doing / responsible for / etc, if it makes sense to have it there.
        Examples:
        ```csharp
        /// <summary>
        /// Does ...
        /// </summary>
        /// <author>Gino</author>
        public class Something { }
        ```
    3. Class Naming Conventions
        - {...}-Panel: Scripts that, in any form, display, create or manipulate UI components
        - {...}-Manager: Scripts that manage an abstract game concept (like Actions, Time, etc.)
        - {...}-Button: Scripts that are attached to UI buttons
        - No script should end with {...}-Script
3. Public Enums
    - Public enums should be placed in a separate file in the Scripts/Enums folder and CAN
    be located in a certain namespace. After every constant, there should be a newline (list style)
4. Constants
    - Enum constants and const values should be written in the SCREAMING_SNAKE_CASE
5. Namespaces
    - The use of namespaces is encouraged in order to organise different parts of the game
6. Bracket Style
    You can use either the next line or same line bracket style:
    1. Next line bracket style
    ```
    NextLine
    {

    }
    ```

    ```
    2. Same line bracket style
    SameLine {

    }
    ```
    
    You should adhere to one of these two styles throughout your classes.
7. Folder Management
    - During development, you should clearly separate between different Assets folders like
    Animation for animation and animator files, Prefabs for prefabs, etc.
8. Git
    - For every subtask, create a new branch (for example: 'tile-building')
    - Instead of working on one big task, divide it into several smaller ones and commit frequently
    - Don't push to main or dev directly. Create a pull request (the dev branch will now be protected)
9. Assertions
    - The use of assertions for inspector references is strongly encouraged
10. Suggestions
    - If you have any suggestions how this stylesheet can be extended, let me know