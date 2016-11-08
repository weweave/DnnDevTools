﻿/**
 * Functionality for Toolbar
 */
(function (document, window, settings) {
    var hub = $.connection.dnnDevToolsNotificationHub,
        currentNoteData = {},
        noteTimeoutId,
        noteElement = document.getElementById('dnnDevTools-note'),
        overviewButton = document.getElementById('dnnDevTools-overviewButton'),
        overlay = document.getElementById('dnnDevTools-overlay'),
        overlayPanel = document.getElementById('dnnDevTools-overlayPanel'),
        overviewIframe = document.getElementById('dnnDevTools-overviewIframe'),
        settingsButton = document.getElementById('dnnDevTools-settingsButton');

    // merge settings with default settings
    settings.noteVisibleTime = settings.noteVisibleTime || 3000;

    // initialization
    initNote();
    initOverlay();
    initConnection();

    // openOverlay({
    //     Type: 'LogMessage',
    //     Id: '52-62-58-76-79-55-F9-29-48-80-95-90-88-F3-6C-89'
    // });

    function initNote() {
        noteElement.addEventListener('click', function () {
            hideNote();
            openOverlay(currentNoteData);
        }, false);
    }

    function initOverlay() {
        // toggle overlay visibility on overview button click
        overviewButton.addEventListener('click', function () {
            toggleOverlay();
        }, false);

        // open settings page on settings button click
        settingsButton.addEventListener('click', function () {
            alert("Please use PersonaBar entry Settings > DNN Dev Tools to configure DNN Dev Tools");
        }, false);

        // close overview window when clicking outside the window panel
        overlay.addEventListener('click', function () {
            closeOverlay();
        });

        // prevent closing the overview window when clicking on the window panel
        overlayPanel.addEventListener('click', function (event) {
            event.stopPropagation();
        });
    }

    function initConnection() {
        // listen to signalR events
        hub.client.OnEvent = function (data) {
            currentNoteData = data;

            // remove icon classes
            noteElement.classList.remove('dnnDevTools-envelopeClosedIcon', 'dnnDevTools-audioIcon', 'dnnDevTools-listIcon');

            switch (data.Type) {
                case 'Mail':
                    noteElement.classList.add('dnnDevTools-envelopeClosedIcon');
                    noteElement.textContent = data.Subject;
                    break;
                case 'DnnEvent':
                    noteElement.classList.add('dnnDevTools-audioIcon');
                    noteElement.textContent = data.Message;
                    break;
                case 'LogMessage':
                    noteElement.classList.add('dnnDevTools-listIcon');
                    noteElement.textContent = data.Message;
                    break;
            }

            showNote();
        };

        // open connection
        $.connection.hub.start();
    }

    function toggleOverlay() {
        if (overlay.classList.contains('dnnDevTools-hidden')) {
            openOverlay();
        } else {
            closeOverlay();
        }
    }

    /**
     * opens the mail overlay
     * @param {string} id The mail with this id will be initially highlighted in overlay
     */
    function openOverlay(noteData) {
        var route = (noteData !== undefined) ? ('#/' + noteData.Type.toLowerCase() + 'detail/' + noteData.Id) : '';

        overviewIframe.src = window.weweave.dnnDevTools.baseUrl + 'Overlay.aspx' + route;
        overlay.classList.remove('dnnDevTools-hidden');

        overviewButton.classList.add('dnnDevTools-active');
    }

    function closeOverlay() {
        overviewIframe.src = '';
        overlay.classList.add('dnnDevTools-hidden');

        overviewButton.classList.remove('dnnDevTools-active');
    }

    function showNote() {
        // TODO check internet explorer support (http://caniuse.com/#search=classList)
        noteElement.classList.remove('dnnDevTools-hidden');

        // reset timeout if another mail comes in while the last mail is still visible
        if (noteTimeoutId) {
            clearTimeout(noteTimeoutId);
            noteTimeoutId = null;
        }

        // hide last mail after some short delay
        noteTimeoutId = window.setTimeout(hideNote, settings.noteVisibleTime);
    }

    function hideNote() {
        // TODO check internet explorer support (http://caniuse.com/#search=classList)
        noteElement.classList.add('dnnDevTools-hidden');
    }
}(document, window, {}));