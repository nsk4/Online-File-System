# sudo pip install git+https://github.com/dpallot/simple-websocket-server.git

from SimpleWebSocketServer import SimpleWebSocketServer, WebSocket
import time
import random
import threading

clients = []

class SimpleEcho(WebSocket):

    def handleMessage(self):
        # echo message back to client
        self.sendMessage("... and I respond to you "+self.data+" as well! -Matevz")

    def handleConnected(self):
        print(self.address, 'connected')
        clients.append(self)

    def handleClose(self):
        print(self.address, 'closed')
        clients.remove(self)

""" Generates random temperature and humidity and sends it to clients. """
def randomMeasurementGenerator():
    while True:
        state = round(random.random()*5)
        for client in clients:
            client.sendMessage(u"{\n  \"state\":"+str(state)+u"\n}")
            time.sleep(2)

t = threading.Thread(target=randomMeasurementGenerator, args=clients)
t.start()

host = '127.0.0.1'
port = 1234
server = SimpleWebSocketServer(host, port, SimpleEcho)
print("Listening on "+host+":"+str(port))
server.serveforever()