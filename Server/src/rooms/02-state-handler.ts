import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Player extends Schema {
    @type("uint16")
    loss = 0;

    @type("uint16")
    kills = 0;

    @type("int8")
    maxHP = 0;

    @type("string")
    currentGun = 'None';

    @type("int8")
    currentHP = 0;
    
    @type("number")
    speed = 0;

    @type("number")
    pX = Math.floor(Math.random() * 25) - 12.5;

    @type("number")
    pY = 20;
    
    @type("number")
    pZ = Math.floor(Math.random() * 25) - 12.5;

    @type("number")
    vX = 0;

    @type("number")
    vY = 0;

    @type("number")
    vZ = 0;

    @type("number")
    rX = 0;

    @type("number")
    rY = 0;

    @type("boolean")
    cr = false;
    
    constructor(data : any){
        super();
        this.speed = data.speed;
        this.maxHP = data.hp;
        this.currentHP = data.hp;
    }
}
export class Loot extends Schema {
    @type("string")
    id = 'LootPistol';
    
    @type("number")
    pX = Math.floor(Math.random() * 25) - 12.5;

    @type("number")
    pY = 10;
    
    @type("number")
    pZ = Math.floor(Math.random() * 25) - 12.5;


    constructor(data: any) {
        super();
        
        this.id = data.id;
        this.pX = data.pX;
        this.pY = data.pY;
        this.pZ = data.pZ;
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    @type({ map: Loot })
    loots = new MapSchema<Loot>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, data: any) {
        this.players.set(sessionId, new Player(data));
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    setMovement (sessionId: string, data: any) {
        const player = this.players.get(sessionId);

        player.pX = data.pX;
        player.pY = data.pY;
        player.pZ = data.pZ;

        player.vX = data.vX;
        player.vY = data.vY;
        player.vZ = data.vZ;

        player.rX = data.rX;
        player.rY = data.rY;
    }
    SetCrouch (sessionId: string, data: any){
        const player = this.players.get(sessionId);
        player.cr = data;
    }

    id = 0;
    CreateUnicId (){
        this.id += 1;
        return this.id.toString();
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 8;

    onCreate (options) {
        this.setPatchRate(100);
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        for (let i = 0; i < 3; i++) {

            const data = {
                pX: Math.floor(Math.random() * 25) - 12.5,
                pY: 30,
                pZ: Math.floor(Math.random() * 25) - 12.5,
                id: "LootPistol"
            };

            this.state.loots.set(this.state.CreateUnicId(), new Loot(data));
        }
        for (let i = 0; i < 3; i++) {

            const data = {
                pX: Math.floor(Math.random() * 25) - 12.5,
                pY: 30,
                pZ: Math.floor(Math.random() * 25) - 12.5,
                id: "LootAutomatic"
            };

            this.state.loots.set(this.state.CreateUnicId(), new Loot(data));
        }
        for (let i = 0; i < 3; i++) {

            const data = {
                pX: Math.floor(Math.random() * 25) - 12.5,
                pY: 30,
                pZ: Math.floor(Math.random() * 25) - 12.5,
                id: "LootGrenadeLauncher"
            };

            this.state.loots.set(this.state.CreateUnicId(), new Loot(data));
        }
        for (let i = 0; i < 3; i++) {

            const data = {
                pX: Math.floor(Math.random() * 25) - 12.5,
                pY: 30,
                pZ: Math.floor(Math.random() * 25) - 12.5,
                id: "LootRocketLauncher"
            };

            this.state.loots.set(this.state.CreateUnicId(), new Loot(data));
        }
        for (let i = 0; i < 3; i++) {

            const data = {
                pX: Math.floor(Math.random() * 25) - 12.5,
                pY: 30,
                pZ: Math.floor(Math.random() * 25) - 12.5,
                id: "LootShotGun"
            };

            this.state.loots.set(this.state.CreateUnicId(), new Loot(data));
        }
        for (let i = 0; i < 3; i++) {

            const data = {
                pX: Math.floor(Math.random() * 25) - 12.5,
                pY: 30,
                pZ: Math.floor(Math.random() * 25) - 12.5,
                id: "LootMinigun"
            };

            this.state.loots.set(this.state.CreateUnicId(), new Loot(data));
        }
        

        this.onMessage("move", (client, data) => {
            console.log("StateHandlerRoom received message from", client.sessionId, ":", data);
            this.state.setMovement(client.sessionId, data);
        });
        this.onMessage("crouch", (client, data) => {
            this.state.SetCrouch(client.sessionId, data);
        });

        this.onMessage("shoot", (client, data) => {
            this.broadcast("Shoot", data, {except: client});
        });

        this.onMessage("pickUp", (client, data) => {
            this.state.loots.delete(data);
        });
        this.onMessage("drop", (client, data) => {
            this.state.loots.set(this.state.CreateUnicId(), new Loot(data));
        });

        this.onMessage("changeGun", (client, data) => {
            const player = this.state.players.get(client.sessionId);
            player.currentGun = data;
        });

        this.onMessage("damage", (client, data) => {
            const damagedClientId = data.id;
            const damagedPlayer = this.state.players.get(damagedClientId);


            let hp = damagedPlayer.currentHP - data.value;
            if (hp > 0) {
                damagedPlayer.currentHP = hp;
                return;
            }
            const player = this.state.players.get(client.sessionId);
            player.kills++;

            damagedPlayer.loss++;
            damagedPlayer.currentHP = damagedPlayer.maxHP;

            for (let i = 0; i < this.clients.length; i++) {
                const client = this.clients[i];
                if (client.id != damagedClientId) {
                    continue;
                }

                const X = Math.floor(Math.random() * 25) - 12.5;
                const Y = 0;
                const Z = Math.floor(Math.random() * 25) - 12.5;

                const message = JSON.stringify({X, Y, Z});
                client.send("Restart", message);
            }
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data : any) {
        client.send("hello", "world");
        this.state.createPlayer(client.sessionId, data);

        if (this.clients.length > 7) {
            this.lock();
        }
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
