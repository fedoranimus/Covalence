import {autoinject, bindable, customElement} from 'aurelia-framework';

@autoinject
@customElement('result-list')
export class ResultListCustomElement { 
    @bindable({ attribute: "matches" }) matches = [];
    @bindable({ attribute: "has-zip" }) hasZip = false;
    needsZeroState = true;

    @bindable({ attribute: "result-type" }) resultType;

    constructor() {

    }

    attached() {

    }
}