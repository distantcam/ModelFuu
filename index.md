---
layout: page
title: ModelFuu
---
{% include JB/setup %}

#{{ site.title }} by {{ site.author.name }}

{{ site.tagline }}

## {{ site.title }} is on Nuget

![PM> Install-Package ModelFuu](assets/ModelFuuNuget.png)

## Details

As a WPF developer I've done my share of [Model-View-ViewModel](http://en.wikipedia.org/wiki/Model_View_ViewModel) development, and one thing I keep stumbling over is how many MVVM examples have you add an implementation of [INotifyPropertyChanged](http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged.aspx) to your Models, and have the ViewModels expose your Model directly to the View.

Of course this can lead to very complex models that support a change notification system that's only required by the UI. What if instead your Models can be [POCO](http://en.wikipedia.org/wiki/Plain_Old_CLR_Object)'s while your ViewModels become responsible for change tracking and notification to your Views?

ModelFuu solves this problem by presenting the UI framework with a ViewModel that has appropriate properties from the Model class without needing the Model to implement anything. No interfaces, no base classes, nothing. Inspired by Paul Stovell's [MicroModels](http://www.paulstovell.com/micromodels-introduction) project and [AutoMapper](http://automapper.org/).

### So, how do you use it?

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class PersonViewModel
    {
        private static ModelPropertyCollection<PersonViewModel, Person> personProperties =
                ModelProperty<PersonViewModel>.CreateFrom<Person>()
                .Build();

        public AutoPersonViewModel(Person person)
        {
            personProperties.LoadFrom(this, person);
        }
    }

*That's it?!?* Aren't you missing INotifyPropertyChanged? Turns out it's not needed for change tracking in WPF. This greatly simplifies things in our ViewModels.

That's great for WPF, but what if I want to change a value in my Model through code and have the ViewModels notice that change?

    personProperties.SetValue(m => m.FirstName, this, "Jimmy");

Or if you want to declare a property in your ViewModel:

    public string FirstName
    {
        get { return (string)personProperties.GetValue(m => m.FirstName, this); }
        set { personProperties.SetValue(m => m.FirstName, this, value); }
    }

Grab ModelFuu from Nuget and give it a go in your next MVVM project.
