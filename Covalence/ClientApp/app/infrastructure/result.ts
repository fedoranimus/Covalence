export class Result implements IResult {
    public firstName: string;
    public lastName: string;

    constructor() {

    }
}

export interface IResult {
    firstName: string;
    lastName: string;
}