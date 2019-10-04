
# Magic Lambda Slots

[![Build status](https://travis-ci.org/polterguy/magic.lambda.slots.svg?master)](https://travis-ci.org/polterguy/magic.lambda.slots)

Provides the ability to create dynamic slots for [Magic](https://github.com/polterguy.magic). More specifically, provides
the following slots.

* __[slots.create]__ - Creates a dynamic slot, that can be invoked with the __[slots.signal]__ slot.
* __[slots.signal]__ - Invokes a dynamically create slot that has been created with the __[slots.create]__ slot.
* __[slots.get]__ - Returns the entire lambda object for a slot that has been previously created with the __[slots.create]__ slot.
* __[slots.delete]__ - Deletes a slot that has been previously created with the __[slots.create]__ slot.
* __[slots.return-nodes]__ - Returns a bunch of nodes to caller from inside of your slot.
* __[slots.return-value]__ - Returns a single value to caller from inside of your slot.
* __[slots.exists]__ - Returns true of the given slot exists.
* __[slots.vocabulary]__ - Returns the names of all dynamically created slots.

Below is an example of how to create and invoke a slot.

```
/*
 * First we create a dynamic slot.
 */
slots.create:foo
   slots.return-value:int:57

/*
 * Then we invoke it.
 */
slots.signal:foo
```

After evaluation of the above Hyperlambda, the value of the __[slots.signal]__ node will be 57. Notice, if you
invoke __[slots.create]__ for a slot that has already been created, the old slot will be overwritten with the
new lambda object you pass into it. Also notice that if you try to invoke a slot that doesn't exist, or you try
to get its content, an exception will be thrown. The __[slots.return-xxx]__ slots, will also throw an exception if you
attempt to invoke them outside a dynamically created slot somehow.

## Rational

This project basically allows you to declare and implement _"globally accessible function objects"_, which you can access from
anywhere in your code. Think of a slot as a _"function"_, and the signaling of a slot like the _"function invocation"_.

## License

Magic is licensed as Affero GPL. This means that you can only use it to create Open Source solutions.
If this is a problem, you can contact at thomas@gaiasoul.com me to negotiate a proprietary license if
you want to use the framework to build closed source code. This will allow you to use Magic in closed
source projects, in addition to giving you access to Microsoft SQL Server adapters, to _"crudify"_
database tables in MS SQL Server. I also provide professional support for clients that buys a
proprietary enabling license.
