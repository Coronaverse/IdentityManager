const nameRegex = /^[\p{L}'.-]+$/;
const ageRegex = /[0-9]+/;

var app = new Vue({
  el: "#app",
  data: {
    showLogin: true,
    showCharacters: false,
    showLoginCharacters: true,
    showLoggingInSpinner: false,
    firstName: "",
    lastName: "",
    birthday: {
      year: new Date().year,
      month: new Date().month,
      day: new Date().day
    },
    dob: "",
    gender: "",
    characters: [],
    errors: [],
  },
  methods: {
	loginCharacter(character) {
	  console.log(`Logging in character ${character.CharacterId}`);
      nfive.send("login", character);
      this.showLoggingInSpinner = true;
    },
    createCharacter() {
      this.showLoginCharacters = false;
    },
    cancelCreateCharacter(event) {
      event.preventDefault();
      this.firstName = "";
      this.lastName = "";
      this.dob = "";
      this.gender = "";
      this.errors = [];
      this.showLoginCharacters = true;
    },
    createCharacterSubmit(event) {
      const self = this;
      event.preventDefault();
      this.errors = [];

      if (this.firstName == "") {
        this.errors.push("firstName");
      }
      if (this.lastName == "") {
        this.errors.push("lastName");
      }
      if (this.dob == "") {
        this.errors.push('dob');
      }
      if (this.gender == "") {
        this.errors.push('gender')
      }

      console.log(this.errors);

      if (this.errors.length > 0) {
        return;
      }
      nfive.send("create", {
        FirstName: this.firstName,
        LastName: this.lastName,
        DateOfBirth: this.dob,
        Gender: this.gender
      });
      this.showLoggingInSpinner = true;
    },
	setCharacters(characters) {
      console.log(`Characters set: ${characters.length}`);
	  this.characters = characters;
    },
    
    eventListener(event) {
      const item = event.data || event.detail;
      if (item.characters) {
        this.characters = item.characters;
        this.showCharacters = true;
        if (this.characters.length < 1) {
          this.showLoginCharacters = false;
        }
      }
      if (item.hasOwnProperty("login")) {
        if (item.login) {
          this.showCharacters = false;
          this.showLogin = false;
        } else {
          this.showLoggingInSpinner = false;
        }
      }
    },
  },
});

nfive.on("nfive:load", () => {
	console.log("NFIVE LOADING ON IDENTITY");
})

nfive.on("characters", (characters) => {
  console.log(characters);
  app.setCharacters(characters);
  app.showCharacters = true;
});

nfive.on("createfailed", () => {
  app.showLoggingInSpinner = false;
});
nfive.show();
