# Sliding-Door
It is a basic sliding door script found in hospitals, homes, workplaces, and almost everywhere.

I was using OnTriggerEnter() and OnTriggerExit() functions in the early stages of the script. When I saw a unique bug where it occurred in crowded places and makes the door shift one more left, I decided to switch to coroutine method.

-It uses an overlap sphere and checks the tags of the objects colliding. You can also visualize this overlap sphere with OnDrawGizmos() code located inside.
-If the door detects somebody approaching, it will be opened, and stay in that form until the last person detected is left, and some time is passed. 
Then, it closes itself.
