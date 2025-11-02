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
    
    constructor(data : any, position: Vector3){
        super();
        this.speed = data.speed;
        this.maxHP = data.hp;
        this.currentHP = data.hp;

        this.pX = position.X;
        this.pY = position.Y;
        this.pZ = position.Z;
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


    constructor(lootId: string, position: Vector3) {
        super();
        
        this.id = lootId;
        this.pX = position.X;
        this.pY = position.Y;
        this.pZ = position.Z;
    }
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    @type({ map: Loot })
    loots = new MapSchema<Loot>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, data: any) {
        const position = this.playerSpawnPoints.GetNextPoint();
        this.players.set(sessionId, new Player(data, position));
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

    CreateLoot(lootId: string, position: Vector3){
        this.loots.set(this.CreateUnicId(), new Loot(lootId, position));
    }

    id = 0;
    CreateUnicId (){
        this.id += 1;
        return this.id.toString();
    }

    playerSpawnPoints: SpawnPoints;
    lootSpawnPoints: SpawnPoints;

    GetRandomLootId(){
    const lootIds = [
        "LootAutomatic", 
        "LootGrenadeLauncher", 
        "LootMinigun",
        "LootPistol",
        "LootRocketLauncher",
        "LootShotGun",
    ];
    const randomIndex = Math.floor(Math.random() * lootIds.length);
    return lootIds[randomIndex];
}
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 8;

    onCreate (options) {
        this.setPatchRate(100);
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        const jsonPlayerSpawnPoints = '{"Points":[{"X":30.200000762939454,"Y":0.8799999952316284,"Z":24.100000381469728},{"X":32.900001525878909,"Y":0.8799999952316284,"Z":-26.399999618530275},{"X":9.720000267028809,"Y":1.3200000524520875,"Z":45.970001220703128},{"X":17.389999389648439,"Y":3.2909998893737795,"Z":-6.130000114440918},{"X":-1.2000000476837159,"Y":3.2909998893737795,"Z":14.0},{"X":-11.5,"Y":12.149999618530274,"Z":-30.469999313354493},{"X":-45.29999923706055,"Y":8.00100040435791,"Z":-13.786999702453614},{"X":-25.393251419067384,"Y":8.079999923706055,"Z":9.670000076293946}]}';
        const parsedPlayerSpawnPoints = JSON.parse(jsonPlayerSpawnPoints);
        this.state.playerSpawnPoints = new SpawnPoints(parsedPlayerSpawnPoints);


        const jsonLootSpawnPoints = '{"Points":[{"X":16.112668991088868,"Y":11.532230377197266,"Z":13.292041778564454},{"X":16.84267234802246,"Y":14.065229415893555,"Z":-10.387959480285645},{"X":-37.597328186035159,"Y":8.525230407714844,"Z":3.9920406341552736},{"X":5.522672653198242,"Y":3.515230178833008,"Z":3.2220401763916017},{"X":11.112668991088868,"Y":3.515230178833008,"Z":8.312040328979493},{"X":3.132669448852539,"Y":3.735229969024658,"Z":-13.107959747314454},{"X":-7.957328796386719,"Y":0.8552298545837402,"Z":30.052040100097658},{"X":16.962671279907228,"Y":0.8552298545837402,"Z":32.02204132080078},{"X":22.41267204284668,"Y":0.8352298736572266,"Z":46.60204315185547},{"X":8.742670059204102,"Y":0.8352298736572266,"Z":35.65203857421875},{"X":49.38267517089844,"Y":0.6352300643920898,"Z":19.592041015625},{"X":52.822669982910159,"Y":2.015230178833008,"Z":-9.847959518432618},{"X":43.582672119140628,"Y":3.6452300548553469,"Z":3.682039260864258},{"X":49.88267517089844,"Y":5.995230197906494,"Z":-0.8879604339599609},{"X":32.322669982910159,"Y":1.1552300453186036,"Z":-5.167960166931152},{"X":41.44267272949219,"Y":0.47523021697998049,"Z":-18.717960357666017},{"X":22.672670364379884,"Y":2.88523006439209,"Z":-38.42795944213867},{"X":-0.0073299407958984379,"Y":11.895230293273926,"Z":-33.32796096801758},{"X":-14.917328834533692,"Y":9.78523063659668,"Z":-23.88796043395996},{"X":-5.837329864501953,"Y":7.765419960021973,"Z":-1.7679595947265626},{"X":-49.55358123779297,"Y":7.765419960021973,"Z":-19.71295928955078},{"X":-21.537328720092775,"Y":7.765419960021973,"Z":-12.817959785461426},{"X":-17.987329483032228,"Y":7.765419960021973,"Z":4.582040786743164},{"X":-25.657329559326173,"Y":9.78523063659668,"Z":-33.127960205078128},{"X":-39.257328033447269,"Y":8.525230407714844,"Z":-16.307960510253908}]}';
        const parsedLootSpawnPoints = JSON.parse(jsonLootSpawnPoints);
        this.state.lootSpawnPoints = new SpawnPoints(parsedLootSpawnPoints);

        for (let i = 0; i < this.state.lootSpawnPoints.Points.length; i++) {
            const position = this.state.lootSpawnPoints.GetNextPoint();
            const lootId = this.state.GetRandomLootId();

            this.state.CreateLoot(lootId, position);
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
            const position = {
                X: data.pX,
                Y: data.pY,
                Z: data.pZ, 
            }
            this.state.CreateLoot(data.id, position);
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

                const position = this.state.playerSpawnPoints.GetNextPoint();

                const X = position.X;
                const Y = position.Y;
                const Z = position.Z;

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
class Vector3 {
    X: number;
    Y: number;
    Z: number;

    constructor(x: number, y: number, z: number) {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
}

class SpawnPoints {
    Points: Vector3[];

    constructor(raw: any) {
        this.Points = raw.Points.map(
            (p: any) => new Vector3(p.X, p.Y, p.Z)
        );
    }

    lastPointIndex = 0;
    GetNextPoint(){
        const lenght = this.Points.length;

        if (this.lastPointIndex >= lenght - 1) {
            this.lastPointIndex = 0;
        }
        else{
            this.lastPointIndex++;
        }

        return this.Points[this.lastPointIndex];
    }
}