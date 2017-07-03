import {autoinject, bindable, customElement} from 'aurelia-framework';
import {DOM} from 'aurelia-pal';

@autoinject
@customElement('tag-editor')
export class TagEditorCustomElement {

    constructor(private element: Element) {

    }

    /******************************
     * Bindings
     ******************************/
    @bindable({ attribute: "available-tags" }) availableTags = [];

    @bindable({ attribute: "debug" }) debug = true;

    @bindable tagQuery = '';

    @bindable suggestedTags = [];

    onAdd(tagName: string = null) {
        if(!tagName)
            tagName = this.tagQuery;

        let event = DOM.createCustomEvent('add', { bubbles: true, cancelable: true, detail: tagName });
        this.element.dispatchEvent(event);
        this.tagQuery = "";
    }

    get hasSuggestions() {
        return this.suggestedTags.length > 0;
    }

    tagQueryChanged(query: string) {
        if(this.debug) console.log(`Query: ${query}`);
        if(query.length > 0) {
            this.suggestedTags = []; //TODO: Call API endpoint
        } else {
            this.suggestedTags = [];
        }
    }
}