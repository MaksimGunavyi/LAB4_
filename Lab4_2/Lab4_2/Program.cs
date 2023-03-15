class Publisher :
    def __init__(self):
        self.subscribers = { }
self.priorities = { }

def register(self, subscriber, priority):
        if priority not in self.subscribers:
            self.subscribers[priority] = []
        self.subscribers[priority].append(subscriber)
        self.priorities[subscriber] = priority

    def unregister(self, subscriber):
        priority = self.priorities.pop(subscriber)
        self.subscribers[priority].remove(subscriber)

    def notify_subscribers(self, event, priority):
        if priority in self.subscribers:
            for subscriber in self.subscribers[priority]:
                subscriber.notify(event)

class Subscriber :
    def __init__(self, name):
        self.name = name

    def notify(self, event):
        print(f"{self.name} received event: {event}")

class HighPrioritySubscriber(Subscriber):
    def __init__(self, name):
        super().__init__(name)

    def notify(self, event):
        print(f"{self.name} received HIGH PRIORITY event: {event}")

class LowPrioritySubscriber(Subscriber):
    def __init__(self, name):
        super().__init__(name)

    def notify(self, event):
        print(f"{self.name} received LOW PRIORITY event: {event}")
