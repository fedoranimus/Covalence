import {computedFrom} from 'aurelia-binding';
import {autoinject, bindable, customElement} from 'aurelia-framework';
import { ITag } from '../../../infrastructure/tag';
import {DOM} from 'aurelia-pal';

@autoinject
@customElement('tag-editor')
export class TagEditorCustomElement {
    @bindable({ attribute: "debug" }) debug = true;
    @bindable({ attribute: "suggested-tags" }) suggestedTags: ITag[] = [];
    @bindable tagQuery = '';

    private fromSelection = false;

    constructor(private element: Element) {

    }

    onAdd(tagName: string = null) {
        if(!tagName)
            tagName = this.tagQuery;

        let event = DOM.createCustomEvent('add', { bubbles: true, cancelable: true, detail: tagName });
        this.element.dispatchEvent(event);
        this.tagQuery = "";
    }
    
    @computedFrom('suggestedTags')
    get hasSuggestions() {
        return this.suggestedTags.length > 0;
    }

    tagQueryChanged(query: string) {
        if(!this.fromSelection) {
            if(this.debug) console.log(`Query: ${query}`);
            if(query.length > 0) {
                let event = DOM.createCustomEvent('change', { bubbles: true, cancelable: true, detail: query });
                this.element.dispatchEvent(event);
            } else {
                this.suggestedTags = [];
            }
        }
        this.fromSelection = false;
    }

    selectTag(tag: ITag) {
        this.fromSelection = true;
        this.tagQuery = tag.name;
        this.suggestedTags = [];
    }
}