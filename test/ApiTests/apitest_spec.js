var frisby = require("frisby");

frisby.create("Check hello world is up")

.get("http://10.28.51.145:5004/")

.expectStatus(200)

.toss();