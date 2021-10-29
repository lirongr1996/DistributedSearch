# DistributedSearch

The algorithm receives from the user text, string to find, number of threads and delta.

It extracts the args to the program and create threadpool with number of threads we given.

We divide the length of the text to 10K buffer. Where each thread gets 10K chars from the text consistency.

We don’t want to cut the searching for the string between two thread so for each thread, except the first one, we save the index of the second appearance of the first char in the string before we move to the next buffer in the global variable, so if it exist we don't miss a potential answer, while we check the previous appearance of the first part in the string.
The next buffer will start from the index of the second appearance of the first char in the string.

Each threat with his parameters will call the method FindString which will receive the budffer, the string to find, the delta.

This method will try to find the string we are looking for.

FindString contain a function called FindSubString which checks if the string continues from the previous buffer to the next one. 
Otherwise, FindString function will search the string in the buffer.

If it found, the method will print the index of the first occurrence of the string in the text and exit the program.

In the end of the text, if all threats finished to run, and the program still running it means that the word we are looking for don’t exist so it will print a massage that the string is not found.


