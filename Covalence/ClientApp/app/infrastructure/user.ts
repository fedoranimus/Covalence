export class User implements IUser {
    private _email: string = "";
    private _firstName: string = "";
    private _lastName: string = "";
    private _studyTags: ITag[] = [];
    private _expertTags: ITag[] = [];
    private _location: Position = null;

    constructor(private userData?: IUserData) {
        if(userData) {
            this._email = userData.email;
            this._firstName = userData.firstName;
            this._lastName = userData.lastName;
            this._studyTags = userData.studyTags;
            this._expertTags = userData.expertTags;
            this._location = userData.location;
        }
    }

    get email() {
        return this._email;
    }

    set email(email:string) {
        this._email = email;
    }

    get firstName() {
        return this._firstName;
    }

    set firstName(firstName: string) {
        this._firstName = firstName;
    }

    get lastName() {
        return this._lastName;
    }

    set lastName(lastName: string) {
        this._lastName = lastName
    }

    get studyTags() {
        return this._studyTags;
    }

    set studyTags(studyTags: ITag[]) {
        this._studyTags = studyTags;
    }

    get expertTags() {
        return this._expertTags;
    }

    set expertTags(expertTags: ITag[]) {
        this._expertTags = expertTags;
    }

    get location() {
        return this._location;
    }

    set location(location: Position) {
        this._location = location;
    }
}

export interface IUserData extends IUser {

}

export interface IUser {
    email: string;
    firstName: string;
    lastName: string;
    studyTags: ITag[];
    expertTags: ITag[];
    location: Position;
}

export interface ITag {
    name: string;
    description: string;
    _id: string;
}