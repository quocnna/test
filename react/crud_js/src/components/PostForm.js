// components/AddPost.js

import React, { Component } from 'react';
import PropTypes from 'prop-types';

class PostForm extends Component {
    static propTypes = {
        addPost: PropTypes.func.isRequired
    }

    render() {
        return (
            <div>
                <h1>
                    Create Post
                </h1>
                <form onSubmit={ this.handleSubmit }>
                    <input required type="text" placeholder="Enter Post Title" ref={ (input) => this.getTitle = input } />
                    <br />
                    <textarea required rows="5" cols="20" placeholder="Enter Post" ref={ (input) => this.getContent = input } />
                    <br />
                    <button>
                        Create
                    </button>
                </form>
            </div>
        );
    }

    handleSubmit = (e) => {
        e.preventDefault();
        const title = this.getTitle.value;
        const content = this.getContent.value;
        this.props.addPost(title, content);
        this.getTitle.value = '';
        this.getContent.value = '';
    }
}

export default PostForm;
