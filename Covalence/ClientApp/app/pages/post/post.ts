import { bindable, autoinject } from 'aurelia-framework';

@autoinject
export class Post {
    @bindable content: string = "";
    constructor() {

    }
}