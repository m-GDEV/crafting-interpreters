# Challenges 

## Introduction

### Question 1

There are at least six domain-specific languages used in the little system 
I cobbled together to write and publish this book. What are they?

#### Answer

domain specific langauges I found

- .gitignore
- Makefile
- bash?
- iml 
- yaml
- .lock
- lox (lol)
- .htaccess
- scss

### Question 2

Get a “Hello, world!” program written and running in Java. Set up whatever makefiles or IDE projects you need to get it working. If you have a debugger, get comfortable with it and step through your program as it runs.

### Answer

Done! But I'm using C#


### Question 3 

Do the same thing for C. To get some practice with pointers, define a doubly linked list of heap-allocated strings. Write functions to insert, find, and delete items from it. Test them.

### Answer 

3. Done. Check part-2 commit: `3947bbcc203a9cc1817f2d02a08c1d28cff9b44c`


## A Map of the Territory

### Question 1 

Pick an open source implementation of a language you like. Download the source code and poke around in it. Try to find the code that implements the scanner and parser. Are they handwritten, or generated using tools like Lex and Yacc? (.l or .y files usually imply the latter.)

### Answer 

I chose to look at the C# compiler: [roslyn](https://github.com/dotnet/roslyn/tree/main/src/Compilers/CSharp/)

This compiler does not seem to use a generated scanner or parse. [Lexer.cs](https://github.com/dotnet/roslyn/blob/main/src/Compilers/CSharp/Portable/Parser/Lexer.cs) and other files in the `Parser` directory indicate scanning / parsing is being done explicitly. This makes sense for a large language like C#

### Question 2 

Just-in-time compilation tends to be the fastest way to implement dynamically typed languages, but not all of them use it. What reasons are there to not JIT?

### Question Answer

Reasons not to use JIT compilation:
- **Application start-up time is critically important**: JIT is slower as it needs to compile as the application runs. This overhead increases start-up time
- **System resource (CPU, RAM) usage is limited and needs to be minimized**: JIT uses more system resources as it caches machine code in RAM and has higher overhead 
- It is harder to implement a JIT compiler and it therefore might not be worth it

The main reson seems to be the increased overhead.

[Source](https://www.javacodegeeks.com/2023/09/just-in-time-jit-compilation-benefits-drawbacks-and-the-evolving-landscape.html)

### Question 3

Most Lisp implementations that compile to C also contain an interpreter that lets them execute Lisp code on the fly as well. Why?

### Answer 

After a few minutes of research the answer is not entirely clear.

I found one stackoverflow post that helped shed some light:

>Lisp is dynamic because both the programming language Lisp and the program itself can be changed at runtime: we can add, change and remove functions, we can add, change or remove syntactic constructs, we can add, change or remove data types (records, classes, ...), we can change the surface syntax of Lisp in various ways, etc. It helps that Lisp is also dynamically typed to provide some of these features.
[Post](https://stackoverflow.com/a/18357903/13646445)

This indicates the while the Common Lisp is normally compiled, the program has 
the ability to dynamically modify itself. 

As far as I can tell lisp implementations include an interpreter so they can 
dynamically execute code. Similar to Python's `eval()` function.

## The Lox Language

### Question 1 

Write some sample Lox programs and run them (you can use the implementations of Lox in my repository). Try to come up with edge case behavior I didn’t specify here. Does it do what you expect? Why or why not?

### Answer

```lox
print "hello world!";

var f = 5 + 5;

print f;

if (true == true) {
    print true;
}

print 0.1 + 0.2;

f = nil;
print f;

f = !nil;

print f;
print "\033[4mmusa";
print !nil;
print !(!nil);

fun beAthing() {
    print "woof i'm a thing!";
    var pp = "pp lol";

    fun beAthing2() {
        print pp;
    }

    beAthing2();
}

beAthing();

{
    {
        {
            {
                {
                    {
                        var ll = "me";
                        print ll;
                    }
                }
            }
        }
    }
}

```

I'm using [this interpreter](https://github.com/zlliang/clox).

Edges cases I noticed:
* The value of '!nil' is always true. Not sure if this common in other languages that use null

### Question 2

This informal introduction leaves a lot unspecified. List several open questions you have about the language’s syntax and semantics. What do you think the answers should be?

### Answer

* This chapter does not mention arrays or lists at all. This would make it hard to use it as a general purpose language
* Are you able to override class methods in derived classes?
* Can you nest blocks inside each other infinitely?
* How are you able to deal with string? Can you iterate through each character? How are they represented? As a character array?

### Question 3

Lox is a pretty tiny language. What features do you think it is missing that would make it annoying to use for real programs? (Aside from the standard library, of course.)

### Answer

* This chapter does not mention arrays or lists at all. This would make it hard to use it as a general purpose language
* There are a lack of built-in data types: lists, arrays, dictionaries, etc 
* foreach loop would be nice 
* Some form of package management / the ability to write, import, and use lox libraries
* Lack of built-in methods on primitive data types (str.split(), etc)

